using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        INoteRepository NoteRepository { get; }
        ILoginAttemptRepository LoginAttemptRepository { get; }
        Task<bool> SaveChangesAsync();
    }
}
