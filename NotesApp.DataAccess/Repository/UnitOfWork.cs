using NotesApp.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _db;
        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            NoteRepository = new NoteRepository(_db);
            UserRepository = new UserRepository(_db);
            LoginAttemptRepository = new LoginAttemptRepository(_db);
        }

        public IUserRepository UserRepository { get; private set; }
        public INoteRepository NoteRepository { get; private set; }
        public ILoginAttemptRepository LoginAttemptRepository { get; private set; }

        public async Task<bool> SaveChangesAsync()
        {
            try
            {
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

            return true;
        }
    }
}
