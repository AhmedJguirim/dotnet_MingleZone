
using System.Security.Cryptography;
using JWT.Algorithms;
using JWT.Serializers;
using JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MingleZone.Models;
using MingleZone.Utils;
using Microsoft.AspNetCore.Authorization;

namespace MingleZone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MingleDbContext _context;
        AuthHelper ah;

        public UsersController(MingleDbContext context)
        {
            _context = context;
            ah =  new AuthHelper();
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // GET: api/userData
        [HttpGet("userdata")]
        public async Task<ActionResult<User>> GetUserData()
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
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(isValid.UserId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                if (_context.Users == null)
                {
                    return Problem("Entity set 'MingleDbContext.Users'  is null.");
                }
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                _context.Users.Add(user);
                Community cm = _context.Communities.Find(1);
                CommunityMembership cmm = new()
                {
                    CommunityId = 1,
                    UserId = user.Id
                };
                user.CommunityMemberships.Add(cmm);
                cm.Memberships.Add(cmm);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {

                        int errorCode = sqlException.Errors[0].Number;
                    if(errorCode == 2601) {
                        return Problem("The email address is already used for another account", statusCode: 500);
                    }
                

                }
                Console.WriteLine(ex);
                return Problem(ex.Message, statusCode: 500);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        [HttpPost("auth")]
        public ActionResult<object> Auth(string email,string password)
        {
            var loginResponse = new LoginResponse { };
            var u = _context.Users.FirstOrDefault(e => e.Email == email);
            if(u == null)
            {
                return Problem("There's no account With this email address",statusCode:404);
            }
            try
            {
                bool valid = BCrypt.Net.BCrypt.Verify(password, u.Password);
            
            
            if (!valid)
            {
                return Problem("Wrong password",statusCode:401);
            }
            }catch(Exception ex)
            {
                return Problem("salt issue", statusCode: 500);
            }
            RSA prkey = ah.LoadPrivateKeyFromPemFile();
            RSA pbkey = ah.LoadPublicKeyFromPemFile();
            IJwtAlgorithm algorithm = new RS256Algorithm(pbkey, prkey);
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            const string key = null; // not needed if algorithm is asymmetric

            IDateTimeProvider provider = new UtcDateTimeProvider();
            var now = provider.GetNow().AddDays(30);
            double secondsSinceEpoch = UnixEpoch.GetSecondsSince(now);
            var payload = new Dictionary<string, object>
            {
                { "id", u.Id },
                { "exp", secondsSinceEpoch }
            };

            string token = encoder.Encode(payload, key);
            loginResponse.Token = token;
            loginResponse.responseMsg = new HttpResponseMessage();

            return Ok( new { loginResponse } ) ;

        }
    }
}


