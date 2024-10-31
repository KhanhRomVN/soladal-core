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

        // api/clones: Create a new clone
        [HttpPost]
        public async Task<ActionResult<Clone>> CreateClone(Clone cloneDto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == cloneDto.GroupId);
                if (group == null || group.Type != cloneDto.Type)
                {
                    return BadRequest("Clone type must match with Group type");
                }

                var clone = new Clone
                {
                    UserId = userId,
                    Type = cloneDto.Type,
                    GroupId = cloneDto.GroupId,
                    Email = cloneDto.Email,
                    Password = cloneDto.Password,
                    TwoFactor = cloneDto.TwoFactor,
                    Phone = cloneDto.Phone,
                    DisplayName = cloneDto.DisplayName,
                    DateOfBirth = cloneDto.DateOfBirth,
                    Country = cloneDto.Country,
                    Language = cloneDto.Language,
                    Agent = cloneDto.Agent,
                    Proxy = cloneDto.Proxy,
                    Status = cloneDto.Status,
                    Notes = cloneDto.Notes,
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

        // api/clones/{id}: Get a clone by ID
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

        // api/clones: Get all clones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Clone>>> GetAllClones()
        {
            try
            {
                int userId = GetUserIdFromToken();
                var clones = await _context.Clones.Where(c => c.UserId == userId).ToListAsync();
                var cloneDTOs = clones.Select(c => new CloneDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Type = c.Type,
                    GroupId = c.GroupId,
                    GroupName = c.GroupId != -1 ? _context.Groups.FirstOrDefault(g => g.Id == c.GroupId)?.Title ?? "" : "",
                    Email = c.Email,
                    Password = c.Password,
                    TwoFactor = c.TwoFactor,
                    Phone = c.Phone,
                    DisplayName = c.DisplayName,
                    DateOfBirth = c.DateOfBirth,
                    Country = c.Country,
                    Language = c.Language,
                    Agent = c.Agent,
                    Proxy = c.Proxy,
                    Status = c.Status,
                    Notes = c.Notes,
                    IsFavorite = c.IsFavorite,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                });
                return Ok(cloneDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // api/clones/group/{groupId}: Get all clones by group ID
        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<IEnumerable<Clone>>> GetClonesByGroup(int groupId)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var clones = await _context.Clones
                    .Where(c => c.UserId == userId && c.GroupId == groupId)
                    .ToListAsync();
                var cloneDTOs = clones.Select(c => new CloneDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Type = c.Type,
                    GroupId = c.GroupId,
                    GroupName = c.GroupId != -1 ? _context.Groups.FirstOrDefault(g => g.Id == c.GroupId)?.Title ?? "" : "",
                    Email = c.Email,
                    Password = c.Password,
                    TwoFactor = c.TwoFactor,
                    Phone = c.Phone,
                    DisplayName = c.DisplayName,
                    DateOfBirth = c.DateOfBirth,
                    Country = c.Country,
                    Language = c.Language,
                    Agent = c.Agent,
                    Proxy = c.Proxy,
                    Status = c.Status,
                    Notes = c.Notes,
                    IsFavorite = c.IsFavorite,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                });
                return Ok(cloneDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // api/clones/favorite/{id}: Change the favorite status of a clone
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

        // api/clones/{id}: Update a clone
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

        // api/clones/{id}: Delete a clone
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