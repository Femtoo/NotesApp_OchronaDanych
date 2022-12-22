using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.NoteService
{
    public interface INoteService
    {
        Task<string> GetFromApi();
        Task<IEnumerable<Note>> GetAllNotes();
        Task AddNote(Note note);
        Task RemoveNote(Note note);
        Task UpdateNote(Note note);
    }
}
