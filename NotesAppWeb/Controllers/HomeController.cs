using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NotesApp.Models;
using NotesApp.Services.NoteService;
using NotesApp.Services.UserService;
using NotesApp.Utility;
using NotesAppWeb.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;

namespace NotesAppWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO userLog)
        {
            var response = await _userService.Login(userLog.Email, userLog.Password);
            if (!response)
            {
                TempData["login_error"] = "Wrong credentials or too many attempts (max 5 attempts in 5 min)";
                return RedirectToAction("Index");
            }

            var user = await _userService.GetUser(u => u.Email == userLog.Email);
            if (user == default)
            {
                TempData["login_error"] = "Wrong credentials or too many attempts (max 5 attempts in 5 min)";
                return RedirectToAction("Index");
            }

            string token = CreateToken(user);

            SetTokenAsCookie(token);

            

            return RedirectToAction("Index", "Notes");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO user)
        {
            var response = await _userService.Register(user.Email, user.Password);
            if (response)
            {
                return RedirectToAction("Index");
            }
            TempData["register_error"] = "There is already account with this email";
            return RedirectToAction("Register");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Logout()
        {
            SetTokenAsCookie("");
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(AuthenticationSettings.JwtKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expires = DateTime.Now.AddDays(AuthenticationSettings.JwtExpiresDays);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void SetTokenAsCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Strict
            };
            Response.Cookies.Append(Constants.XAccessToken, token, cookieOptions);
        }
    }
}