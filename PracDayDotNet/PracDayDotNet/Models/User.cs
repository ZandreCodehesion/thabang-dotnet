using System;
using System.ComponentModel.DataAnnotations;

namespace PracDay.Models
{
    public class User
    {
        public Guid UserId { get; set; } 
        
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
