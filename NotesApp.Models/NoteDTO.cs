using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Models
{
    public class NoteDTO
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        //[Required]
        //public bool IsPublic { get; set; } = false;
        public int UserId { get; set; }
    }
}
