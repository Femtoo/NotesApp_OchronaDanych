using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Models.ViewModels
{
    public class PasswordVM
    {
        public int NoteId { get; set; }
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
