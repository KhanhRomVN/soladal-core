using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Register: api/users
        [HttpPost]
        public async Task<ActionResult<string>> Register(User user)
        {
            // Check if username or email already exists
            if (await _context.Users.AnyAsync(u => u.Username == user.Username))
            {
                return BadRequest("Username is already taken.");
            }

            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("Email is already registered.");
            }

            // Hash the password before saving
            user.Password = HashPassword(user.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok("Registration completed successfully.");
        }

        // Login: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserLogin user)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == user.Email);

            if (existingUser == null || !BCrypt.Net.BCrypt.Verify(user.Password, existingUser.Password))
            {
                return Unauthorized();
            }

            // Generate token
            var authToken = new AuthToken();
            var token = authToken.GenerateToken(existingUser);
            return Ok(new { access_token = token });
        }

        // Get user by id: api/users/<id>
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            var token = Request.Headers["access_token"].ToString();
            var authToken = new AuthToken();
            var claims = authToken.VerifyToken(token);

            var userIdClaim = claims.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim.Value);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }


        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}