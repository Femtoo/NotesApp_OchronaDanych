using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesApp.DataAccess.Repository.IRepository;
using NotesApp.Models;
using System.Security.Claims;
using System.Text.Json;

namespace NotesAppWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NotesApiController(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = contextAccessor;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string result = "notes";
            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateNote(Note note)
        {


            return Ok();
        }

        [HttpGet("allnotes")]
        public async Task<IActionResult> GetAllNotes()
        {
            string result = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == result, includeProperties: "Notes");

            if(user != default)
            {
                var json = JsonSerializer.Serialize(user.Notes);
                return Ok(json);
            } else
            {
                return BadRequest();
            }
        }
    }
}
