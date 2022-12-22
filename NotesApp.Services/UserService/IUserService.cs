using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.UserService
{
    public interface IUserService
    {
        Task<bool> Register(string email, string password);
        Task<bool> Login(string username, string password);
        Task<User> GetUser(Expression<Func<User, bool>> filter, string? includeProperties = null);
    }
}
