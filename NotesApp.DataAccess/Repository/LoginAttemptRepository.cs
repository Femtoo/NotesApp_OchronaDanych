using NotesApp.DataAccess.Repository.IRepository;
using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.DataAccess.Repository
{
    public class LoginAttemptRepository : Repository<LoginAttempt>, ILoginAttemptRepository
    {
        private readonly AppDbContext _context;
        public LoginAttemptRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
