using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MingleZone.Models;
using MingleZone.Models.outputModels;
using MingleZone.Utils;

namespace MingleZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunitiesController : ControllerBase
    {
        AuthHelper ah;
        private readonly MingleDbContext _context;

        public CommunitiesController(MingleDbContext context)
        {
            _context = context;
            ah = new AuthHelper();
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
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Token not provided");
            }
            if (!authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid token format");
            }
            string jwtToken = authorizationHeader.Substring("Bearer ".Length);

            TokenValidationResult isValid = ah.ValidateToken(jwtToken);
            if (!isValid.IsTokenValid)
            {
                return Unauthorized(isValid.ErrorMessage);
            }
            if (id != community.Id)
            {
                return BadRequest();
            }
            if(isValid.UserId != community.AdminId)
            {
                return Unauthorized("only the owner of the community can update it");
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
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Token not provided");
            }
            if (!authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid token format");
            }
            string jwtToken = authorizationHeader.Substring("Bearer ".Length);

            TokenValidationResult isValid = ah.ValidateToken(jwtToken);
            if (!isValid.IsTokenValid)
            {
                return Unauthorized(isValid.ErrorMessage);
            }

            if (_context.Communities == null)
          {
              return Problem("Entity set 'MingleDbContext.Communities'  is null.", statusCode: 500);
          }
            
            User admin = _context.Users.FirstOrDefault(x => x.Id == isValid.UserId);
            if(admin == null)
            {
                return Problem("the Id in the token belongs to nobody in the database", statusCode: 401);
            }

            community.AdminId = admin.Id;
            if (admin != null)
            {
                _context.Communities.Add(community);

                await _context.SaveChangesAsync();
            }

            else
            {
                return Problem("Admin user not found.",statusCode:404);
            }
            CommunityMembership cm = new()
            {
                UserId = admin.Id,
                CommunityId = community.Id
            };
            _context.CommunityMemberships.Add(cm);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Communities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Token not provided");
            }
            if (!authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid token format");
            }
            string jwtToken = authorizationHeader.Substring("Bearer ".Length);

            TokenValidationResult isValid = ah.ValidateToken(jwtToken);
            if (!isValid.IsTokenValid)
            {
                return Unauthorized(isValid.ErrorMessage);
            }

            if (_context.Communities == null)
            {
                return Problem("Entity set 'MingleDbContext.Communities'  is null.", statusCode: 500);
            }
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }
            if(community.AdminId != isValid.UserId)
            {
                return Unauthorized("Only the owner of the community can delete it");
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("getMembershipRequests/{CommunityId}")]
        public async Task<ActionResult<IEnumerable<RequestsOut>>> GetMembershipRequests(int CommunityId)
        {
            if (_context.MembershipRequests == null)
            {
                return NotFound();
            }
            var result = await _context.MembershipRequests.Where(r => r.Id == CommunityId).Select(r => new RequestsOut
            {
                requestId = r.Id,
                UserId = r.User.Id,
                UserName = r.User.Name,
            }).ToListAsync();
            return result;
        }

        [HttpPost("requestMembership/{id}")]
        public async Task<ActionResult<Community>> RequestMembership(int id)

        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Token not provided");
            }
            if (!authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid token format");
            }
            string jwtToken = authorizationHeader.Substring("Bearer ".Length);

            TokenValidationResult isValid = ah.ValidateToken(jwtToken);
            if (!isValid.IsTokenValid)
            {
                return Unauthorized(isValid.ErrorMessage);
            }

            if (_context.Communities == null)
            {
                return Problem("Entity set 'MingleDbContext.Communities'  is null.", statusCode: 500);
            }

            User user = _context.Users.FirstOrDefault(x => x.Id == isValid.UserId);
            if (user == null)
            {
                return Problem("the Id in the token belongs to nobody in the database", statusCode: 401);
            }

            var membershipExists = _context.CommunityMemberships
        .Any(cm => cm.UserId == isValid.UserId && cm.CommunityId == id);
            if (membershipExists)
            {
                return Problem("you are already a member", statusCode:403);
            }
            MembershipRequest mr = new()
            {
                CommunityId = id,
                UserId = isValid.UserId,
            };
            _context.MembershipRequests.Add(mr);
            await _context.SaveChangesAsync();
            return Ok();
        }
        public class MembershipHelper
        {
            public int Id { get; set; }
            public int AdminId { get; set; }
        }
        [HttpPost("approveMembership")]
        public async Task<ActionResult<Community>> ApproveMembership(int id)

        {
            string authorizationHeader = HttpContext.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(authorizationHeader))
            {
                return Unauthorized("Token not provided");
            }
            if (!authorizationHeader.StartsWith("Bearer "))
            {
                return Unauthorized("Invalid token format");
            }
            string jwtToken = authorizationHeader.Substring("Bearer ".Length);

            TokenValidationResult isValid = ah.ValidateToken(jwtToken);
            if (!isValid.IsTokenValid)
            {
                return Unauthorized(isValid.ErrorMessage);
            }

            if (_context.Communities == null)
            {
                return Problem("Entity set 'MingleDbContext.Communities'  is null.", statusCode: 500);
            }

            User admin = _context.Users.FirstOrDefault(x => x.Id == isValid.UserId);
            if (admin == null)
            {
                return Problem("the Id in the token belongs to nobody in the database", statusCode: 401);
            }

            var mr = _context.MembershipRequests
        .Where(r => r.Id == id).Select(r => new MembershipHelper
        {
            Id = r.Id,
            AdminId = r.Community.AdminId
        }).FirstOrDefault();
            if (mr == null)
            {
                return NotFound("request not found");
            }
            if (mr.AdminId != admin.Id)
            {
                return Unauthorized("only the admin can accept requests for his community");
            }
            CommunityMembership cm = new()
            {
                CommunityId = id,
                UserId = isValid.UserId,
            };
            MembershipRequest req = _context.MembershipRequests.Find(id);
            _context.CommunityMemberships.Add(cm);
            _context.MembershipRequests.Remove(req);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private bool CommunityExists(int id)
        {
            return (_context.Communities?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
