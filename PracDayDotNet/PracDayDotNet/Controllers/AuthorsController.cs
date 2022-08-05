using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PracDay.Data;
using PracDayDotNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Identity.Web;
using PracDay.Models;
using Microsoft.Identity.Client;
using PracDayDotNet.Data;
using System.IdentityModel.Tokens.Jwt;

namespace PracDayDotNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        internal Connection _con = new Connection();

        private static TokenClass token = new TokenClass();

        
        [HttpPost("createAuthor")]
        public int CreateAuthor([FromBody] Authors authors)
        {
            int control = 2;
            //First check if author exists
            Authors existAuthor = _con.openConnection().QueryFirstOrDefault<Authors>(
                @"SELECT * FROM dbo.Authors WHERE AuthorName='"+authors.AuthorName+"'");

            if(existAuthor == null)
            {
                authors.CreatedBy = getAuthorId();
                    try
                    {
                        control = _con.openConnection().Execute(@"INSERT INTO dbo.Authors 
                            VALUES('" + authors.AuthorName + "','" + authors.ActiveFrom + "','" + authors.ActiveTo + "'," +
                           "'" + authors.CreatedBy + "')");
                    }catch(Exception e)
                    {
                        control = -1;
                        e.GetBaseException();
                    }
                
            }else if(existAuthor != null)
            {
                control = 0;
            }
            return control;
        }

        private static string getAuthorId()
        {
            //Decypher JWT
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(token.getToken());
            string authorId = decodedValue.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return authorId;
        }


        [HttpGet("getAllAuthors")]
        public IEnumerable<Authors> getAllAuthors()
        {
            IEnumerable<Authors> authors = _con.openConnection().Query<Authors>(@"
            SELECT * FROM dbo.Authors");

            _con.closeConnection();

            return authors;
        }

        [HttpGet("getAuthor/{id}")]
        public Authors getAuthor(int id)
        {
            Authors author = _con.openConnection().QueryFirstOrDefault<Authors>(
                @"SELECT * FROM dbo.Authors WHERE AuthorId="+id+"");

            _con.closeConnection();

            return author;
        }

        [HttpPatch("updateAuthor/{id}")]
        public int updateAuthor([FromBody] Authors update, int id)
        {
            int control = 2;

            Authors existAuthor = _con.openConnection().QueryFirstOrDefault<Authors>
                (@"SELECT * FROM dbo.Authors WHERE AuthorId=" + id + "");

            if(existAuthor!= null)
            {
                try
                {


                    control = _con.openConnection().Execute(@"
                                UPDATE dbo.Authors
                                SET AuthorName='" + update.AuthorName + "'," +
                                    "ActiveFrom='" + update.ActiveFrom + "'," +
                                    "ActiveTo='" + update.ActiveTo + "' " +
                                    "WHERE AuthorId='" + id + "'");
                }
                catch (Exception e)
                {
                    control = -1;
                    e.GetBaseException();
                }
            }
            else
            {
                control = 0;
            }

            _con.closeConnection();

            return control;

        }

        [HttpDelete("deleteAuthor/{id}")]
        public int deleteAuthor(int id)
        {
            Authors existAuthor = _con.openConnection().QueryFirstOrDefault<Authors>(
                @"SELECT * FROM dbo.Authors WHERE AuthorId=" + id + "");
            int control = 2;

            if(existAuthor!= null)
            {
                try
                {
                    control = _con.openConnection().Execute(
                        @"DELETE FROM dbo.Authors 
                            WHERE AuthorId=" + id + "");
                }catch(Exception e)
                {
                    control = -1;
                    e.GetBaseException();
                }
            }
            else
            {
                control = 0;
            }

            return control;
        }

    }
}
