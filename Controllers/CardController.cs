using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soladal_core.Data;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;

        public CardsController(ApplicationDbContext context)
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

        // Create card: /api/cards
        [HttpPost]
        public async Task<ActionResult<Card>> CreateCard(Card cardDto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == cardDto.GroupId);
                if (group == null || group.Type != cardDto.Type)
                {
                    return BadRequest("Card type must match with Group type");
                }

                var card = new Card
                {
                    UserId = userId,
                    Title = cardDto.Title ?? "",
                    Type = cardDto.Type,
                    GroupId = cardDto.GroupId,
                    FullName = cardDto.FullName ?? "",
                    CardNumber = cardDto.CardNumber ?? "",
                    ExpirationDate = cardDto.ExpirationDate ?? "",
                    Pin = cardDto.Pin ?? "",
                    Notes = cardDto.Notes ?? "",
                    IsFavorite = cardDto.IsFavorite
                };

                _context.Cards.Add(card);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCardById), new { id = card.Id }, card);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get card by id: /api/cards/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCardById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (card == null)
                {
                    return NotFound();
                }

                return card;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get all cards: /api/cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetAllCards()
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Cards.Where(c => c.UserId == userId).ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get cards by group: /api/cards/group/{group_id}
        [HttpGet("group/{group_id}")]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsByGroup(int group_id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Cards
                    .Where(c => c.UserId == userId && c.GroupId == group_id)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get cards by title: /api/cards/title/{title}
        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<Card>>> GetCardsByTitle(string title)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Cards
                    .Where(c => c.UserId == userId && c.Title.Contains(title))
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Change favorite status: /api/cards/favorite/{id}
        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<Card>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (card == null)
                {
                    return NotFound();
                }

                card.IsFavorite = isFavorite;
                card.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return card;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Update card: /api/cards/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Card>> UpdateCard(int id, Card card)
        {
            try
            {
                int userId = GetUserIdFromToken();
                if (id != card.Id || userId != card.UserId)
                {
                    return BadRequest();
                }

                card.UpdatedAt = DateTime.Now;
                _context.Entry(card).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CardExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return card;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Delete card: /api/cards/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Card>> DeleteCard(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var card = await _context.Cards.FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

                if (card == null)
                {
                    return NotFound();
                }

                _context.Cards.Remove(card);
                await _context.SaveChangesAsync();

                return card;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        private bool CardExists(int id)
        {
            return _context.Cards.Any(e => e.Id == id);
        }
    }
}