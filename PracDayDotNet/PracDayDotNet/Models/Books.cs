using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracDayDotNet.Models
{
    public class Books
    {
        public Guid BookId { get; set; }
        public string BookName { get; set; }
        public string Publisher { get; set; }
        public DateTime DatePublished { get; set; }
        public int CopiesSold { get; set; }
        public string AuthorId { get; set; } //GUID(AuthorId)
        public string CreatedBy{ get; set; } //GUID(UserId)
}
}
