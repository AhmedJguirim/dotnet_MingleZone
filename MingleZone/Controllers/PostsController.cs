using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class PostsController : ControllerBase
    {
        private readonly MingleDbContext _context;
        AuthHelper ah;

        public PostsController(MingleDbContext context)
        {
            _context = context;
            ah = new AuthHelper();
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostsOut>>> GetPosts()
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
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
            int currentUserId = isValid.UserId;
            var communityIds = _context.CommunityMemberships
                .Where(m => m.UserId == currentUserId)
                .Select(m => m.CommunityId)
                .ToList();
            var posts = _context.Posts.Where(p => communityIds.Contains(p.CommunityId)).Include(p => p.User).Include(p => p.Community).Select(p => new PostsOut
            {
                Id = p.Id,
                Content = p.Content,
                UserId = p.UserId,
                UserName = p.User.Name,
                CommunityId = p.Community.Id,
                CommunityName = p.Community.Name
            }).ToList();
            return posts;
        }
        // GET: api/Posts
        [HttpGet("/Posts/user/{userId}")]
        public async Task<ActionResult<IEnumerable<PostsOut>>> GetPostsByUser(int userId)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
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
            int currentUserId = isValid.UserId;
            var communityIds = _context.CommunityMemberships
                .Where(m => m.UserId == currentUserId)
                .Select(m => m.CommunityId)
                .ToList();
            var posts = _context.Posts.Where(p => communityIds.Contains(p.CommunityId) && p.UserId==userId).Include(p => p.User).Include(p => p.Community).Select(p => new PostsOut
            {
                Id = p.Id,
                Content = p.Content,
                UserId = p.UserId,
                UserName = p.User.Name,
                CommunityId = p.Community.Id,
                CommunityName = p.Community.Name
            }).ToList();
            return posts;
        }
        [HttpGet("/community/posts/{CommunityId}")]
        public async Task<ActionResult<IEnumerable<PostsOut>>> GetCommunityPosts(int CommunityId)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
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
            int currentUserId = isValid.UserId;

            bool isMember = _context.CommunityMemberships.Any(cm=>cm.UserId == currentUserId && cm.CommunityId == CommunityId);
            if(!isMember)
            {
                return Unauthorized("you need to be a member of this community to be able to get its posts");
            }
            var posts = _context.Posts.Where(p => p.CommunityId==CommunityId).Include(p => p.User).Include(p => p.Community).Select(p => new PostsOut
            {
                Id = p.Id,
                Content = p.Content,
                UserId = p.UserId,
                UserName = p.User.Name,
                CommunityId = p.Community.Id,
                CommunityName = p.Community.Name
            }).ToList();
            return posts;
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostsOut>> GetPost(int id)
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            var post = _context.Posts.Where(p => p.Id ==id).Include(p => p.User).Include(p => p.Community).Select(p => new PostsOut
            {
                Id = p.Id,
                Content = p.Content,
                UserId = p.UserId,
                UserName = p.User.Name,
                CommunityId = p.Community.Id,
                CommunityName = p.Community.Name
            }).FirstOrDefault();

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, Post post)
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
            if (id != post.Id)
            {
                return BadRequest();
            }
            if(post.UserId!=isValid.UserId)
            {
                return Unauthorized("only the owner of a post can update it");
            }

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Post>> PostPost(Post post)
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
            if (_context.Posts == null)
          {
              return Problem("Entity set 'MingleDbContext.Posts'  is null.");
          }
            bool isMember = _context.CommunityMemberships.Any(m => m.CommunityId == post.UserId);
            if(!isMember)
            {
                return Unauthorized("you cannot post in this community , become a member first");
            }
            post.UserId = isValid.UserId;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
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


            var post = await _context.Posts.FindAsync(id);
            var community = await _context.Communities.FindAsync(post.CommunityId);
            if (post == null)
            {
                return NotFound();
            }
            if (post.UserId != isValid.UserId && isValid.UserId != community.AdminId)
            {
                return Unauthorized("only the owner of a post or the community's admin can delete it");
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
