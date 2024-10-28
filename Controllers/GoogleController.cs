using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GooglesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;

        public GooglesController(ApplicationDbContext context)
        {
            _context = context;
            _authToken = new AuthToken();
        }

        private int GetUserIdFromToken()
        {
            var token = Request.Headers["access_token"].ToString();
            var claims = _authToken.VerifyToken(token);
            var userIdClaim = claims.FindFirst("UserId");
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException();
            }
            return int.Parse(userIdClaim.Value);
        }

        [HttpPost]
        public async Task<ActionResult<Googles>> CreateGoogle(Googles googleDto)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = new Googles
                {
                    UserId = userId,
                    Type = googleDto.Type,
                    GroupId = googleDto.GroupId,
                    Email = googleDto.Email ?? "",
                    Phone = googleDto.Phone ?? "",
                    Password = googleDto.Password ?? "",
                    Country = googleDto.Country ?? "",
                    Agent = googleDto.Agent ?? "",
                    TwoFactor = googleDto.TwoFactor ?? "",
                    IsFavorite = googleDto.IsFavorite
                };

                _context.Googles.Add(google);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGoogleById), new { id = google.Id }, google);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Googles>> GetGoogleById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = await _context.Googles.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

                if (google == null)
                {
                    return NotFound();
                }

                return google;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Googles>>> GetAllGoogles()
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Googles.Where(g => g.UserId == userId).ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("group/{group_id}")]
        public async Task<ActionResult<IEnumerable<Googles>>> GetGooglesByGroup(int group_id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Googles
                    .Where(g => g.UserId == userId && g.GroupId == group_id)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<Googles>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = await _context.Googles.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

                if (google == null)
                {
                    return NotFound();
                }

                google.IsFavorite = isFavorite;
                google.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return google;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Googles>> UpdateGoogle(int id, Googles google)
        {
            try
            {
                int userId = GetUserIdFromToken();
                if (id != google.Id || userId != google.UserId)
                {
                    return BadRequest();
                }

                google.UpdatedAt = DateTime.Now;
                _context.Entry(google).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoogleExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return google;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Googles>> DeleteGoogle(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = await _context.Googles.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

                if (google == null)
                {
                    return NotFound();
                }

                _context.Googles.Remove(google);
                await _context.SaveChangesAsync();

                return google;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        private bool GoogleExists(int id)
        {
            return _context.Googles.Any(e => e.Id == id);
        }
    }
}