using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Models.ViewModels
{
    public class NoteCreateVM
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords have to match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
