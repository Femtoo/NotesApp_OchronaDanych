using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NotesApp.DataAccess.Repository.IRepository;
using NotesApp.Models;
using NotesApp.Models.ViewModels;
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
        private readonly IPasswordHasher<NoteDTO> _passwordHasher;
        public NotesApiController(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, IPasswordHasher<NoteDTO> passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = contextAccessor;
            _passwordHasher = passwordHasher;
        }

        [HttpPost("updatenote")]
        public async Task<IActionResult> UpdateNote([FromBody]NoteDTO note)
        {
            string email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email, includeProperties: "Notes");
            if (user == null)
            {
                return BadRequest();
            }

            if (!user.Notes.Select(u => u.Id).Contains(note.Id))
            {
                return BadRequest();
            }

            await _unitOfWork.NoteRepository.Update(new Note
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
            });

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("addnote")]
        public async Task<IActionResult> CreateNote([FromBody]NoteDTO note)
        {
            string email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return BadRequest();
            }

            _unitOfWork.NoteRepository.Add(new Note
            {
                Title = note.Title,
                Content = note.Content,
                PasswordHash = note.PasswordHash,
                User = user,
            });

            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("allnotes")]
        public async Task<IActionResult> GetAllNotes()
        {
            string email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email, includeProperties: "Notes");
            List<NoteDTO> notes = new List<NoteDTO>();

            if(user == null)
            {
                return BadRequest();
            }

            foreach (var note in user.Notes)
            {
                notes.Add(new NoteDTO
                {
                    Id = note.Id,
                    Title = note.Title,
                });
            }

            return Ok(notes);
        }

        [HttpPost("getnote")]
        public async Task<IActionResult> GetNote([FromBody]int id)
        {
            string email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email, includeProperties: "Notes");

            if (user == null)
            {
                return BadRequest();
            }

            if (!user.Notes.Select(u => u.Id).Contains(id))
            {
                return BadRequest();
            }

            var note = user.Notes.FirstOrDefault(u => u.Id == id);

            if(note == null )
            {
                return BadRequest();
            }

            NoteDTO result = new NoteDTO
            {
                Id = note.Id,
                Content = note.Content,
                PasswordHash = note.PasswordHash,
                Title = note.Title,
                UserId = user.Id,
            };

            return Ok(result);
        }

        [HttpPost("getnotehash")]
        public async Task<IActionResult> GetNoteHash([FromBody] int id)
        {
            string email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email, includeProperties: "Notes");

            if (user == null)
            {
                return BadRequest();
            }

            if (!user.Notes.Select(u => u.Id).Contains(id))
            {
                return BadRequest();
            }

            var note = user.Notes.FirstOrDefault(u => u.Id == id);

            if (note == null)
            {
                return BadRequest();
            }

            return Ok(note.PasswordHash);
        }

        [HttpPost("deletenote")]
        public async Task<IActionResult> DeleteNote([FromBody] int id)
        {
            string email = string.Empty;
            if (_httpContextAccessor.HttpContext != null)
            {
                email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            }
            var user = await _unitOfWork.UserRepository.GetFirstOrDefault(u => u.Email == email, includeProperties: "Notes");

            if (user == null)
            {
                return BadRequest();
            }

            if (!user.Notes.Select(u => u.Id).Contains(id))
            {
                return BadRequest();
            }

            var note = user.Notes.FirstOrDefault(u => u.Id == id);

            if (note == null)
            {
                return BadRequest();
            }

            _unitOfWork.NoteRepository.Remove(note);
            var result = await _unitOfWork.SaveChangesAsync();

            return Ok(result);
        }
    }
}
