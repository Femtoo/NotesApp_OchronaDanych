using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesApp.Models
{
    public class LoginAttempt
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string User { get; set; } = string.Empty;
        [Required]
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
