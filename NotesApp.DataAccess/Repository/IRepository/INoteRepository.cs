using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataAccess.Repository.IRepository
{
    public interface INoteRepository : IRepository<Note>
    {
        Task<bool> Update(Note note);
    }
}
