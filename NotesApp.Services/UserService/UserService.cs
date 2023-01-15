using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using NotesApp.DataAccess.Repository.IRepository;
using NotesApp.Models;
using NotesApp.Services.LoginAttemptService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ILoginAttemptService _loginAttemptService;
        public UserService(IUnitOfWork unitOfWork, IPasswordHasher<User> passwordHasher, ILoginAttemptService loginAttemptService)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _loginAttemptService = loginAttemptService;
        }

        public async Task<User> GetUser(Expression<Func<User, bool>> filter, string? includeProperties = null)
        {
            var userDb = await _unitOfWork.UserRepository.GetFirstOrDefault(filter, includeProperties);
            return userDb;
        }

        public async Task<bool> Login(string email, string password)
        {
            if(await _loginAttemptService.CheckIfLoginAttemptShouldBeBlocked(email))
            {
                return false;
            }
            await _loginAttemptService.AddLoginAttempt(email);
            var userDB = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email);
            if (userDB == default)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(userDB, userDB.PasswordHash, password);
            if(result == PasswordVerificationResult.Failed)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> Register(string email, string password)
        {
            var userDB = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email);
            if (userDB != default)
            {
                return false;
            }
            User newUser = new User
            {
                Email = email,
            };
            var hashed = _passwordHasher.HashPassword(newUser, password);
            newUser.PasswordHash = hashed;
            _unitOfWork.UserRepository.Add(newUser);
            var response = await _unitOfWork.SaveChangesAsync();
            return response;
        }

    }
}
