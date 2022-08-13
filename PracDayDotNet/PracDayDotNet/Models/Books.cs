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
        public Guid Author { get; set; } //GUID(AuthorId)
        public Guid CreatedBy{ get; set; } //GUID(UserId)
}
}
