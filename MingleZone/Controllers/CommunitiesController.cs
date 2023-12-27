using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MingleZone.Models;

namespace MingleZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        private readonly MingleDbContext _context;

        public CommunitiesController(MingleDbContext context)
        {
            _context = context;
        }

        // GET: api/Communities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
        {
          if (_context.Communities == null)
          {
              return NotFound();
          }
            return await _context.Communities.ToListAsync();
        }

        // GET: api/Communities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
          if (_context.Communities == null)
          {
              return NotFound();
          }
            var community = await _context.Communities.FindAsync(id);

            if (community == null)
            {
                return NotFound();
            }

            return community;
        }

        // PUT: api/Communities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, Community community)
        {
            if (id != community.Id)
            {
                return BadRequest();
            }

            _context.Entry(community).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Communities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Community>> PostCommunity(Community community)
        {
          if (_context.Communities == null)
          {
              return Problem("Entity set 'MingleDbContext.Communities'  is null.", statusCode: 500);
          }
            User admin = _context.Users.FirstOrDefault(x => x.Id == community.AdminId);
            if (admin != null)
            {
                _context.Communities.Add(community);
                await _context.SaveChangesAsync();
            }
            else
            {
                return Problem("Admin user not found.",statusCode:404);
            }
            community.Admin = admin;

            return CreatedAtAction("GetCommunity", new { id = community.Id }, community);
        }

        // DELETE: api/Communities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            if (_context.Communities == null)
            {
                return NotFound();
            }
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityExists(int id)
        {
            return (_context.Communities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
