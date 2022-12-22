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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> Update(User user)
        {
            var userDb = await GetFirstOrDefault(u => u.Id == user.Id);
            if (userDb == default)
            {
               return false;
            }
            _context.Users.Attach(user);
            _context.Entry(user).State = EntityState.Modified;
            return true;
        }
    }
}
