using System;

namespace PracDay.Models
{
    public class User
    {
        public Guid UserId { get; set; } 
        
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
