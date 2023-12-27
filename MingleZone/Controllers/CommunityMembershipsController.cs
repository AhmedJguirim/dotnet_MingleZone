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
    public class CommunityMembershipsController : ControllerBase
    {
        private readonly MingleDbContext _context;

        public CommunityMembershipsController(MingleDbContext context)
        {
            _context = context;
        }

        // GET: api/CommunityMemberships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommunityMembership>>> GetCommunityMemberships()
        {
          if (_context.CommunityMemberships == null)
          {
              return NotFound();
          }
            return await _context.CommunityMemberships.ToListAsync();
        }

        // GET: api/CommunityMemberships/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommunityMembership>> GetCommunityMembership(int id)
        {
          if (_context.CommunityMemberships == null)
          {
              return NotFound();
          }
            var communityMembership = await _context.CommunityMemberships.FindAsync(id);

            if (communityMembership == null)
            {
                return NotFound();
            }

            return communityMembership;
        }

        // PUT: api/CommunityMemberships/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunityMembership(int id, CommunityMembership communityMembership)
        {
            if (id != communityMembership.Id)
            {
                return BadRequest();
            }

            _context.Entry(communityMembership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityMembershipExists(id))
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

        // POST: api/CommunityMemberships
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CommunityMembership>> PostCommunityMembership(CommunityMembership communityMembership)
        {
          if (_context.CommunityMemberships == null)
          {
              return Problem("Entity set 'MingleDbContext.CommunityMemberships'  is null.");
          }
            _context.CommunityMemberships.Add(communityMembership);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCommunityMembership", new { id = communityMembership.Id }, communityMembership);
        }

        // DELETE: api/CommunityMemberships/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunityMembership(int id)
        {
            if (_context.CommunityMemberships == null)
            {
                return NotFound();
            }
            var communityMembership = await _context.CommunityMemberships.FindAsync(id);
            if (communityMembership == null)
            {
                return NotFound();
            }

            _context.CommunityMemberships.Remove(communityMembership);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityMembershipExists(int id)
        {
            return (_context.CommunityMemberships?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
