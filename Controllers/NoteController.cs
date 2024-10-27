// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;

// namespace soladal_core.Controllers
// {
//     [Route("api/[controller]")]
//     [ApiController]
//     public class NoteController : ControllerBase
//     {
//         private readonly ApplicationDbContext _context;

//         public NoteController(ApplicationDbContext context)
//         {
//             _context = context;
//         }

//         [HttpPost]
//         public async Task<ActionResult<Note>> CreateNote(Note note)
//         {
//             _context.Notes.Add(note);
//             await _context.SaveChangesAsync();
//             return CreatedAtAction(nameof(GetNoteByID), new { id = note.Id }, note);
//         }

//         [HttpGet("{id}")]
//         public async Task<ActionResult<Note>> GetNoteByID(int id)
//         {
//             var note = await _context.Notes.FindAsync(id);
//             if (note == null)
//             {
//                 return NotFound();
//             }
//             return note;
//         }

//         [HttpGet]
//         public async Task<ActionResult<IEnumerable<Note>>> GetAllNotes()
//         {
//             return await _context.Notes.ToListAsync();
//         }

//         [HttpGet("group/{group}")]
//         public async Task<ActionResult<IEnumerable<Note>>> GetNoteByGroup(string group)
//         {
//             return await _context.Notes.Where(n => n.Group == group).ToListAsync();
//         }

//         [HttpGet("title/{title}")]
//         public async Task<ActionResult<IEnumerable<Note>>> GetNoteByTitle(string title)
//         {
//             return await _context.Notes.Where(n => n.Title.Contains(title)).ToListAsync();
//         }

//         [HttpGet("group/{group}/title/{title}")]
//         public async Task<ActionResult<IEnumerable<Note>>> GetNoteByGroupAndTitle(string group, string title)
//         {
//             return await _context.Notes.Where(n => n.Group == group && n.Title.Contains(title)).ToListAsync();
//         }

//         [HttpPut("favorite/{id}")]
//         public async Task<ActionResult<Note>> ChangeFavoriteStatus(int id, bool isFavorite)
//         {
//             var note = await _context.Notes.FindAsync(id);
//             if (note == null)
//             {
//                 return NotFound();
//             }
//             note.IsFavorite = isFavorite;
//             note.UpdatedAt = DateTime.Now;
//             await _context.SaveChangesAsync();
//             return note;
//         }

//         [HttpPut("{id}")]
//         public async Task<ActionResult<Note>> UpdateNote(int id, Note note)
//         {
//             if (id != note.Id)
//             {
//                 return BadRequest();
//             }
//             _context.Entry(note).State = EntityState.Modified;
//             note.UpdatedAt = DateTime.Now;
//             try
//             {
//                 await _context.SaveChangesAsync();
//             }
//             catch (DbUpdateConcurrencyException)
//             {
//                 if (!NoteExists(id))
//                 {
//                     return NotFound();
//                 }
//                 else
//                 {
//                     throw;
//                 }
//             }
//             return note;
//         }

//         [HttpDelete("{id}")]
//         public async Task<ActionResult<Note>> DeleteNote(int id)
//         {
//             var note = await _context.Notes.FindAsync(id);
//             if (note == null)
//             {
//                 return NotFound();
//             }
//             _context.Notes.Remove(note);
//             await _context.SaveChangesAsync();
//             return note;
//         }

//         private bool NoteExists(int id)
//         {
//             return _context.Notes.Any(e => e.Id == id);
//         }
//     }
// }