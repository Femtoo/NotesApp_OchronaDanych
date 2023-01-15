using Microsoft.AspNetCore.Identity;
using NotesApp.DataAccess.Repository.IRepository;
using NotesApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.LoginAttemptService
{
    public class LoginAttemptService : ILoginAttemptService
    {
        private readonly IUnitOfWork _unitOfWork;
        public LoginAttemptService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddLoginAttempt(string username)
        {
            _unitOfWork.LoginAttemptRepository.Add(new LoginAttempt { User = username });
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CheckIfLoginAttemptShouldBeBlocked(string username)
        {
            var results = await _unitOfWork.LoginAttemptRepository.GetAll(u => u.Date > DateTime.Now.AddMinutes(-5) && u.User.Equals(username));
            if(results.Count() > 5)
            {
                return true;
            }
            return false;
        }
    }
}
