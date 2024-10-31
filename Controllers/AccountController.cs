using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;

        public AccountsController(ApplicationDbContext context)
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

        // Create account: /api/accounts
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(Account accountDto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == accountDto.GroupId);
                if (group == null || group.Type != accountDto.Type)
                {
                    return BadRequest("Account type must match with Group type");
                }

                var account = new Account
                {
                    UserId = userId,
                    Title = accountDto.Title,
                    GroupId = accountDto.GroupId,
                    Website_URL = accountDto.Website_URL,
                    Username = accountDto.Username ?? "",
                    Email = accountDto.Email ?? "",
                    Phone = accountDto.Phone ?? "",
                    Password = accountDto.Password ?? "",
                    IsFavorite = accountDto.IsFavorite,
                    Notes = accountDto.Notes ?? "",
                    Type = accountDto.Type,
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetAccountByID), new { id = account.Id }, account);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get account by id: /api/accounts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccountByID(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (account == null)
                {
                    return NotFound();
                }

                return account;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get all accounts: /api/accounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            try
            {
                int userId = GetUserIdFromToken();
                var accountData = await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
                var accountDTOs = accountData.Select(a => new AccountDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Title = a.Title,
                    Type = a.Type,
                    GroupId = a.GroupId,
                    GroupName = a.GroupId != -1 ? _context.Groups.FirstOrDefault(g => g.Id == a.GroupId)?.Title ?? "" : "",
                    Website_URL = a.Website_URL,
                    Username = a.Username,
                    Email = a.Email,
                    Phone = a.Phone,
                    Password = a.Password,
                    TwoFactor = a.TwoFactor,
                    Notes = a.Notes,
                    IsFavorite = a.IsFavorite,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                });
                return Ok(accountDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get accounts by group id: /api/accounts/group/{group_id}
        [HttpGet("group/{group_id}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountByGroup(int group_id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var accountData = await _context.Accounts.Where(a => a.UserId == userId && a.GroupId == group_id).ToListAsync();
                var accountDTOs = accountData.Select(a => new AccountDTO
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Title = a.Title,
                    Type = a.Type,
                    GroupId = a.GroupId,
                    GroupName = a.GroupId != -1 ? _context.Groups.FirstOrDefault(g => g.Id == a.GroupId)?.Title ?? "" : "",
                    Website_URL = a.Website_URL,
                    Username = a.Username,
                    Email = a.Email,
                    Phone = a.Phone,
                    Password = a.Password,
                    TwoFactor = a.TwoFactor,
                    Notes = a.Notes,
                    IsFavorite = a.IsFavorite,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                });
                return Ok(accountDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get accounts by title: /api/accounts/title/{title}
        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountByTitle(string title)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Accounts
                    .Where(a => a.UserId == userId && a.Title.Contains(title))
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get accounts by group and title: /api/accounts/group/{group}/title/{title}
        [HttpGet("group/{group}/title/{title}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountByGroupAndTitle(string group, string title)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Accounts
                    .Where(a => a.UserId == userId && a.GroupId.ToString() == group && a.Title.Contains(title))
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Change favorite status: /api/accounts/favorite/{id}
        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<Account>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (account == null)
                {
                    return NotFound();
                }

                account.IsFavorite = isFavorite;
                account.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return account;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Update account: /api/accounts/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Account>> UpdateAccount(int id, Account account)
        {
            try
            {
                int userId = GetUserIdFromToken();
                Console.WriteLine(account);
                if (id != account.Id || userId != account.UserId)
                {
                    return BadRequest();
                }

                account.UpdatedAt = DateTime.Now;
                _context.Entry(account).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return account;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Delete account: /api/accounts/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Account>> DeleteAccount(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);

                if (account == null)
                {
                    return NotFound();
                }

                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                return account;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}