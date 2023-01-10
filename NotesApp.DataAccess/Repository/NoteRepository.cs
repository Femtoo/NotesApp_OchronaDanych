using Microsoft.EntityFrameworkCore;
using NotesApp.DataAccess.Repository.IRepository;
using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataAccess.Repository
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        private readonly AppDbContext _context;
        public NoteRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Update(Note note)
        {
            var noteDb = await GetFirstOrDefault(u => u.Id == note.Id);
            if (noteDb == default)
            {
                return false;
            }
            noteDb.Title = note.Title;
            noteDb.Content = note.Content;
            _context.Notes.Attach(noteDb);
            _context.Entry(noteDb).State = EntityState.Modified;
            return true;
        }
    }
}
