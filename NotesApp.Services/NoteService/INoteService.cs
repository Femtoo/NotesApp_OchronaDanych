using NotesApp.Models;
using NotesApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.NoteService
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDTO>> GetAllNotes();
        Task AddNote(NoteDTO note);
        Task<bool> RemoveNote(int id);
        Task UpdateNote(NoteDTO note);
        Task<NoteDTO?> GetNote(PasswordVM password);
    }
}
