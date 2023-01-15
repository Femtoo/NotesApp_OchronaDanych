using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.LoginAttemptService
{
    public interface ILoginAttemptService
    {
        Task AddLoginAttempt(string username);
        Task<bool> CheckIfLoginAttemptShouldBeBlocked(string username);
    }
}
