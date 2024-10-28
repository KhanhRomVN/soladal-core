using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soladal_core.Data;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClonesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;

        public ClonesController(ApplicationDbContext context)
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
        public async Task<ActionResult<Clone>> CreateClone(Clone cloneDto)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var clone = new Clone
                {
                    UserId = userId,
                    Type = cloneDto.Type,
                    GroupId = cloneDto.GroupId,
                    Email = cloneDto.Email ?? "",
                    Password = cloneDto.Password ?? "",
                    TwoFactor = cloneDto.TwoFactor ?? "",
                    Agent = cloneDto.Agent ?? "",
                    Proxy = cloneDto.Proxy ?? "",
                    Country = cloneDto.Country ?? "",
                    Status = cloneDto.Status ?? "",
                    IsFavorite = cloneDto.IsFavorite
                };

                _context.Clones.Add(clone);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCloneById), new { id = clone.Id }, clone);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Clone>> GetCloneById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var clone = await _context.Clones.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (clone == null)
                {
                    return NotFound();
                }

                return clone;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clone>>> GetAllClones()
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Clones.Where(c => c.UserId == userId).ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("group/{group_id}")]
        public async Task<ActionResult<IEnumerable<Clone>>> GetClonesByGroup(int group_id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Clones
                    .Where(c => c.UserId == userId && c.GroupId == group_id)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<Clone>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var clone = await _context.Clones.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (clone == null)
                {
                    return NotFound();
                }

                clone.IsFavorite = isFavorite;
                clone.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return clone;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Clone>> UpdateClone(int id, Clone clone)
        {
            try
            {
                int userId = GetUserIdFromToken();
                if (id != clone.Id || userId != clone.UserId)
                {
                    return BadRequest();
                }

                clone.UpdatedAt = DateTime.Now;
                _context.Entry(clone).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CloneExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return clone;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Clone>> DeleteClone(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var clone = await _context.Clones.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (clone == null)
                {
                    return NotFound();
                }

                _context.Clones.Remove(clone);
                await _context.SaveChangesAsync();

                return clone;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        private bool CloneExists(int id)
        {
            return _context.Clones.Any(e => e.Id == id);
        }
    }
}