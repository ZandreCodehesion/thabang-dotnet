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
using Microsoft.Extensions.Configuration;

namespace PracDayDotNet.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class AuthorsController : ControllerBase
    {

        private IConfiguration _config;

        public AuthorsController(IConfiguration config)
        {
            _config = config;
        }
        internal Connection _con = new Connection();

        private static TokenClass token = new TokenClass();

        [Authorize]
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
                            VALUES('" + authors.AuthorName + "','" + authors.ActiveFrom.Date + "','" + authors.ActiveTo.Date + "'," +
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


        [Authorize]
        [HttpGet("getAllAuthors")]
        public IEnumerable<Authors> getAllAuthors()
        {
            IEnumerable<Authors> authors = _con.openConnection().Query<Authors>(@"
            SELECT * FROM dbo.Authors");

            _con.closeConnection();

            return authors;
        }

        [Authorize]
        [HttpGet("getAuthor/{id}")]
        public Authors getAuthor(int id)
        {
            Authors author = _con.openConnection().QueryFirstOrDefault<Authors>(
                @"SELECT * FROM dbo.Authors WHERE AuthorId="+id+"");

            _con.closeConnection();

            return author;
        }



        [Authorize]
        [HttpPatch("updateAuthor/{id}")]
        public int updateAuthor([FromBody] Authors update, int id)
        {
            int control = 2;

            //Check if logged in user is the same as what I am to delete
            string LoggedUser = getAuthorId();

            
            IEnumerable<Authors> authorList = _con.openConnection().Query<Authors>
                (@"SELECT * FROM dbo.Authors WHERE CreatedBy='" + LoggedUser + "'");

            if(authorList!= null)
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


        [Authorize]
        [HttpDelete("deleteAuthor/{id}")]
        public int deleteAuthor(int id)
        {
            string loggedUser = getAuthorId();

            List<Authors> existAuthor = _con.openConnection().QueryFirstOrDefault<List<Authors>>(
                @"SELECT * FROM dbo.Authors WHERE CreatedBy=" + loggedUser + "");

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
