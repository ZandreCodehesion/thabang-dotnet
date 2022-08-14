using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracDayDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using PracDay.Data;
using PracDayDotNet.Data;
using Microsoft.Extensions.Configuration;

namespace PracDayDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private Connection _con = new Connection();
        private TokenClass token = new TokenClass();

        private IConfiguration _config;

        public BooksController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("CreateBook/{AuthorId}")]
        public int CreateBook([FromBody]Books book,string AuthorId)
        {
            int control = 2;
            Books existBook = _con.openConnection().QueryFirstOrDefault<Books>(
                $"SELECT * FROM dbo.Books WHERE BookName='{book.BookName}'"); 
            
            if (existBook == null)
            {
                try
                {
                    control = _con.openConnection().Execute($"INSERT INTO dbo.Books " +
                        $"VALUES('{Guid.NewGuid()}','{book.BookName}','{book.Publisher}','{book.DatePublished.Date}',{book.CopiesSold}," +
                        $"'{AuthorId}','{token.getAuthorId()}')",book);
                }catch(Exception e)
                {
                    control = -1;
                    e.GetBaseException();
                }

            }else if(existBook != null)
            {
                control = 0;
            }

            _con.closeConnection();
            return control;
        }

        [HttpGet("GetAllAuthorBooks")]
        public IEnumerable<BookName_Author> GetAllBooks()
        {
            IEnumerable<BookName_Author> books = _con.openConnection().Query<BookName_Author>($"SELECT BookName,AuthorName " +
                $"FROM dbo.Books  LEFT JOIN dbo.Authors " +
                $"ON dbo.Books.AuthorId = dbo.Authors.AuthorId");
            _con.closeConnection();
            return books;
        }

        [HttpGet("GetBooksByAuthorId/{AuthorId}")]
        public IEnumerable<AuthorBooks_Published>GetAuthorBooks(string AuthorId)
        {
            IEnumerable<AuthorBooks_Published> authorBooks = _con.openConnection().Query<AuthorBooks_Published>
                ($"SELECT BookName, DatePublished FROM dbo.Books WHERE AuthorId='{AuthorId}'");
            _con.closeConnection();
            return authorBooks;
        }

        [HttpGet("GetBook/{AuthorId}/{BookId}")]
        public Books getBook(string AuthorId,string BookId)
        {
            Books book = _con.openConnection().QueryFirstOrDefault<Books>(
                $"SELECT BookName,DatePublished,Publisher,CopiesSold,AuthorName " +
                $"FROM dbo.Books LEFT JOIN dbo.Authors " +
                $"ON dbo.Books.AuthorId = dbo.Authors.AuthorId");
            _con.closeConnection();

            return book;
        }
    }

}
