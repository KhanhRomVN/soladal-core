using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soladal_core.Data;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;

        public IdentitiesController(ApplicationDbContext context)
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
        public async Task<ActionResult<Identity>> CreateIdentity(Identity identityDto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == identityDto.GroupId);
                if (group == null || group.Type != identityDto.Type)
                {
                    return BadRequest("Identity type must match with Group type");
                }

                var identity = new Identity
                {
                    UserId = userId,
                    Type = identityDto.Type,
                    GroupId = identityDto.GroupId,
                    Firstname = identityDto.Firstname ?? "",
                    Lastname = identityDto.Lastname ?? "",
                    DateOfBirth = identityDto.DateOfBirth,
                    Gender = identityDto.Gender ?? "",
                    Country = identityDto.Country ?? "",
                    City = identityDto.City ?? "",
                    Street = identityDto.Street ?? "",
                    Zipcode = identityDto.Zipcode ?? "",
                    PassportID = identityDto.PassportID ?? "",
                    PassportIssuedBy = identityDto.PassportIssuedBy ?? "",
                    PassportIssuedDate = identityDto.PassportIssuedDate,
                    PassportExpiredDate = identityDto.PassportExpiredDate,
                    IDCardID = identityDto.IDCardID ?? "",
                    IDCardIssuedBy = identityDto.IDCardIssuedBy ?? "",
                    IDCardIssuedDate = identityDto.IDCardIssuedDate,
                    IDCardExpiredDate = identityDto.IDCardExpiredDate,
                    DrivingLicenseID = identityDto.DrivingLicenseID ?? "",
                    DrivingLicenseIssuedBy = identityDto.DrivingLicenseIssuedBy ?? "",
                    DrivingLicenseIssuedDate = identityDto.DrivingLicenseIssuedDate,
                    DrivingLicenseExpiredDate = identityDto.DrivingLicenseExpiredDate,
                    Phone = identityDto.Phone ?? "",
                    Gmail = identityDto.Gmail ?? "",
                    PasswordGmail = identityDto.PasswordGmail ?? "",
                    TwoFactorGmail = identityDto.TwoFactorGmail ?? "",
                    JobTitle = identityDto.JobTitle ?? "",
                    JobCompany = identityDto.JobCompany ?? "",
                    JobDescription = identityDto.JobDescription ?? "",
                    JobStartDate = identityDto.JobStartDate,
                    JobEndDate = identityDto.JobEndDate,
                    IsFavorite = identityDto.IsFavorite
                };

                _context.Identities.Add(identity);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetIdentityById), new { id = identity.Id }, identity);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Identity>> GetIdentityById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var identity = await _context.Identities.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

                if (identity == null)
                {
                    return NotFound();
                }

                return identity;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Identity>>> GetAllIdentities()
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Identities.Where(i => i.UserId == userId).ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("group/{group_id}")]
        public async Task<ActionResult<IEnumerable<Identity>>> GetIdentitiesByGroup(int group_id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Identities
                    .Where(i => i.UserId == userId && i.GroupId == group_id)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<Identity>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var identity = await _context.Identities.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

                if (identity == null)
                {
                    return NotFound();
                }

                identity.IsFavorite = isFavorite;
                identity.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return identity;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Identity>> UpdateIdentity(int id, Identity identity)
        {
            try
            {
                int userId = GetUserIdFromToken();
                if (id != identity.Id || userId != identity.UserId)
                {
                    return BadRequest();
                }

                identity.UpdatedAt = DateTime.Now;
                _context.Entry(identity).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IdentityExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return identity;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Identity>> DeleteIdentity(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var identity = await _context.Identities.FirstOrDefaultAsync(i => i.Id == id && i.UserId == userId);

                if (identity == null)
                {
                    return NotFound();
                }

                _context.Identities.Remove(identity);
                await _context.SaveChangesAsync();

                return identity;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        private bool IdentityExists(int id)
        {
            return _context.Identities.Any(e => e.Id == id);
        }
    }
}