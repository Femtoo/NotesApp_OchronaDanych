using Microsoft.AspNetCore.Mvc;
using NotesApp.Models;
using NotesApp.Models.ViewModels;
using NotesApp.Services.NoteService;

namespace NotesAppWeb.Controllers
{
    public class NotesController : Controller
    {

        private readonly INoteService _noteService;
        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        public async Task<IActionResult> Index()
        {
            List<NotesIndexVM> notesIndex = new List<NotesIndexVM>();
            IEnumerable<Note> notes = await _noteService.GetAllNotes();
            foreach (Note note in notes)
            {
                notesIndex.Add(new NotesIndexVM
                {
                    Title = note.Title,
                    Id = note.Id,
                });
            }
            return View(notesIndex);
        }
    }
}
