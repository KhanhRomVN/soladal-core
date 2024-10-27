using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using soladal_core.Data;

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

        private static string GenerateTitleFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }
            string domain = url.Replace("http://", "").Replace("https://", "");

            domain = domain.Replace("www.", "");

            int indexOfSlash = domain.IndexOf('/');
            if (indexOfSlash != -1)
            {
                domain = domain.Substring(0, indexOfSlash);
            }

            string[] commonExtensions = { ".com", ".net", ".org", ".edu", ".gov", ".co" };
            foreach (var ext in commonExtensions)
            {
                if (domain.EndsWith(ext, StringComparison.OrdinalIgnoreCase))
                {
                    domain = domain.Substring(0, domain.Length - ext.Length);
                    break;
                }
            }

            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(domain);
        }

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("group/{group}")]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccountByGroup(string group)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Accounts
                    .Where(a => a.UserId == userId && a.GroupId.ToString() == group)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

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

        [HttpPut("{id}")]
        public async Task<ActionResult<Account>> UpdateAccount(int id, Account account)
        {
            try
            {
                int userId = GetUserIdFromToken();
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