using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;
        public GroupController(ApplicationDbContext context)
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
        public async Task<ActionResult<Group>> CreateGroup(Group group)
        {
            int userId = GetUserIdFromToken();
            var groupData = new Group { UserId = userId, Title = group.Title, CanDelete = false, LucideIcon = group.LucideIcon };
            _context.Groups.Add(groupData);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAllGroupByUserId), new { id = groupData.Id }, groupData);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetAllGroupByUserId()
        {
            int userId = GetUserIdFromToken();
            return await _context.Groups.Where(g => g.UserId == userId).ToListAsync();
        }

        [HttpPut("addItem/{id}")]
        public async Task<ActionResult<Account>> AddItemToGroup(int id, int groupId)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            account.GroupId = groupId;
            account.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return account;
        }

        [HttpPut("removeItem/{id}")]
        public async Task<ActionResult<Account>> RemoveItemFromGroup(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            account.GroupId = 0; 
            account.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return account;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Group>> UpdateGroup(int id, Group group)
        {
            if (id != group.Id)
            {
                return BadRequest();
            }
            _context.Entry(group).State = EntityState.Modified;
            group.UpdatedAt = DateTime.Now;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return group;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Group>> DeleteGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();
            return group;
        }

        private bool GroupExists(int id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}