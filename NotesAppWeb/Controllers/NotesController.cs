using Ganss.Xss;
using Markdig;
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
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<NotesIndexVM> notesIndex = new List<NotesIndexVM>();
            IEnumerable<NoteDTO> notes = await _noteService.GetAllNotes();
            foreach (NoteDTO note in notes)
            {
                notesIndex.Add(new NotesIndexVM
                {
                    Title = note.Title,
                    Id = note.Id,
                });
            }
            return View(notesIndex);
        }

        public IActionResult CreateNote()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNote(NoteCreateVM note)
        {

            if(ModelState.IsValid)
            {
                var sanitizer = new HtmlSanitizer();
                await _noteService.AddNote(new NoteDTO
                {
                    Id = 0,
                    Title = note.Title,
                    Content = sanitizer.Sanitize(note.Content),
                    PasswordHash = note.Password,
                    UserId = 0
                });
                return RedirectToAction("Index");
            }

            return View(note);
        }
        public IActionResult CreateCommonNote()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCommonNote(NoteCommonVM note)
        {

            if (ModelState.IsValid)
            {
                var sanitizer = new HtmlSanitizer();
                await _noteService.AddNote(new NoteDTO
                {
                    Id = 0,
                    Title = note.Title,
                    Content = sanitizer.Sanitize(note.Content),
                    UserId = 0
                });
                return RedirectToAction("Index");
            }

            return View(note);
        }

        public async Task<IActionResult> Password(int id)
        {
            if(await _noteService.CheckIfCommonNote(id))
            {
                return RedirectToAction("ShowCommon", new PasswordVM
                {
                    NoteId=id,
                    Password="common"
                });
            }
            return View(new PasswordVM { NoteId = id, Password = string.Empty});
        }

        public async Task<IActionResult> ShowCommon(PasswordVM pass)
        {
            var note = await _noteService.GetNote(pass);
            if (note == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                var sanitizer = new HtmlSanitizer();
                note.Content= sanitizer.Sanitize(Markdown.ToHtml(note.Content));
                return View("Show",note);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Show(PasswordVM pass)
        {
            var note = await _noteService.GetNote(pass);
            if (note == null)
            {
                TempData["wrong_pass"] = "Wrong Password";
                return RedirectToAction("Index");
            }
            else
            {
                var sanitizer = new HtmlSanitizer();
                note.Content = sanitizer.Sanitize(Markdown.ToHtml(note.Content));
                note.PasswordHash = pass.Password;
                return View(note);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(NoteDTO note)
        {
            var notee = await _noteService.GetNote(new PasswordVM { NoteId = note.Id, Password = note.PasswordHash});
            if(notee != null)
            {
                note.Content = notee.Content;
                return View(note);
            }
            return View(note);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePOST(NoteDTO note)
        {
            if(ModelState.IsValid)
            {
                var sanitizer = new HtmlSanitizer();
                note.Content = sanitizer.Sanitize(note.Content);
                await _noteService.UpdateNote(note);
                return RedirectToAction("Index");
            } else
            {

            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _noteService.RemoveNote(id);
            return RedirectToAction("Index");
        }
    }
}
