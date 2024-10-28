using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soladal_core.Data;

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

        // api/googles: Create a new Google account
        [HttpPost]
        public async Task<ActionResult<GoogleAccount>> CreateGoogle(GoogleAccount googleDto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == googleDto.GroupId);
                if (group == null || group.Type != googleDto.Type)
                {
                    return BadRequest("Google account type must match with Group type");
                }

                var google = new GoogleAccount
                {
                    UserId = userId,
                    Type = googleDto.Type,
                    GroupId = googleDto.GroupId,
                    Email = googleDto.Email,
                    Password = googleDto.Password,
                    RecoveryEmail = googleDto.RecoveryEmail,
                    TwoFactor = googleDto.TwoFactor,
                    Phone = googleDto.Phone,
                    DisplayName = googleDto.DisplayName,
                    DateOfBirth = googleDto.DateOfBirth,
                    Country = googleDto.Country,
                    Language = googleDto.Language,
                    Agent = googleDto.Agent,
                    Proxy = googleDto.Proxy,
                    Status = googleDto.Status,
                    Notes = googleDto.Notes,
                    IsFavorite = googleDto.IsFavorite
                };

                _context.GoogleAccounts.Add(google);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetGoogleById), new { id = google.Id }, google);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // api/googles/{id}: Get a Google account by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GoogleAccount>> GetGoogleById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = await _context.GoogleAccounts.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

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

        // api/googles: Get all Google accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoogleAccount>>> GetAllGoogles()
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.GoogleAccounts.Where(g => g.UserId == userId).ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // api/googles/group/{groupId}: Get all Google accounts by group ID
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IEnumerable<GoogleAccount>>> GetGooglesByGroup(int groupId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.GoogleAccounts
                    .Where(g => g.UserId == userId && g.GroupId == groupId)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // api/googles/favorite/{id}: Change the favorite status of a Google account
        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<GoogleAccount>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = await _context.GoogleAccounts.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

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

        // api/googles/{id}: Update a Google account
        [HttpPut("{id}")]
        public async Task<ActionResult<GoogleAccount>> UpdateGoogle(int id, GoogleAccount google)
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

        // api/googles/{id}: Delete a Google account
        [HttpDelete("{id}")]
        public async Task<ActionResult<GoogleAccount>> DeleteGoogle(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var google = await _context.GoogleAccounts.FirstOrDefaultAsync(g => g.Id == id && g.UserId == userId);

                if (google == null)
                {
                    return NotFound();
                }

                _context.GoogleAccounts.Remove(google);
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
            return _context.GoogleAccounts.Any(e => e.Id == id);
        }
    }
}