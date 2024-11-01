using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using soladal_core.Data;

namespace soladal_core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuthToken _authToken;

        public NotesController(ApplicationDbContext context)
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

        // Create a new note: /api/notes
        [HttpPost]
        public async Task<ActionResult<Note>> CreateNote(Note noteDto)
        {
            try
            {
                int userId = GetUserIdFromToken();

                var group = await _context.Groups.FirstOrDefaultAsync(g => g.Id == noteDto.GroupId);
                if (group == null || group.Type != noteDto.Type)
                {
                    return BadRequest("Note type must match with Group type");
                }

                var note = new Note
                {
                    UserId = userId,
                    Title = noteDto.Title,
                    Type = noteDto.Type,
                    GroupId = noteDto.GroupId,
                    Notes = noteDto.Notes ?? "",
                    IsFavorite = noteDto.IsFavorite
                };

                _context.Notes.Add(note);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get a note by id: /api/notes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNoteById(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

                if (note == null)
                {
                    return NotFound();
                }

                return note;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get all notes: /api/notes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllNotes()
        {
            try
            {
                int userId = GetUserIdFromToken();
                var notes = await _context.Notes.Where(n => n.UserId == userId).ToListAsync();
                var noteDTOs = notes.Select(n => new NoteDTO
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Title = n.Title,
                    Type = n.Type,
                    GroupId = n.GroupId,
                    GroupName = n.GroupId != -1 ? _context.Groups.FirstOrDefault(gr => gr.Id == n.GroupId)?.Title ?? "" : "",
                    Notes = n.Notes,
                    IsFavorite = n.IsFavorite,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt
                });
                return Ok(noteDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get notes by group id: /api/notes/group/{group_id}
        [HttpGet("group/{group_id}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByGroup(int group_id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                return await _context.Notes
                    .Where(n => n.UserId == userId && n.GroupId == group_id)
                    .ToListAsync();
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Get notes by title: /api/notes/title/{title}
        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotesByTitle(string title)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var notes = await _context.Notes
                    .Where(n => n.UserId == userId && n.Title.Contains(title))
                    .ToListAsync();
                var noteDTOs = notes.Select(n => new NoteDTO
                {
                    Id = n.Id,
                    UserId = n.UserId,
                    Title = n.Title,
                    Type = n.Type,
                    GroupId = n.GroupId,
                    GroupName = n.GroupId != -1 ? _context.Groups.FirstOrDefault(gr => gr.Id == n.GroupId)?.Title ?? "" : "",
                    Notes = n.Notes,
                    IsFavorite = n.IsFavorite,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt
                });
                return Ok(noteDTOs);
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Change favorite status: /api/notes/favorite/{id}
        [HttpPut("favorite/{id}")]
        public async Task<ActionResult<Note>> ChangeFavoriteStatus(int id, bool isFavorite)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

                if (note == null)
                {
                    return NotFound();
                }

                note.IsFavorite = isFavorite;
                note.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return note;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Update a note: /api/notes/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<Note>> UpdateNote(int id, Note note)
        {
            try
            {
                int userId = GetUserIdFromToken();
                if (id != note.Id || userId != note.UserId)
                {
                    return BadRequest();
                }

                note.UpdatedAt = DateTime.Now;
                _context.Entry(note).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return note;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        // Delete a note: /api/notes/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<Note>> DeleteNote(int id)
        {
            try
            {
                int userId = GetUserIdFromToken();
                var note = await _context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId);

                if (note == null)
                {
                    return NotFound();
                }

                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();

                return note;
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}