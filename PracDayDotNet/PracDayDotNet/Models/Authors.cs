using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracDayDotNet.Models
{
    public class Authors
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public string  CreatedBy { get; set; }
    }
}
