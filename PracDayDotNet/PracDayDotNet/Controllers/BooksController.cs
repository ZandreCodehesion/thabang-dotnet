using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracDayDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracDayDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpPost("CreateBook/{AuthorId}")]
        public int CreateBook([FromBody]Books book,int AuthorId)
        {

        }
    }
}
