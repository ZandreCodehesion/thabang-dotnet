using Microsoft.AspNetCore.Mvc;
using PracDay.Data;
using PracDay.Models;
using Dapper;
using System;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PracDay.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        internal Connection connection = new Connection();
        private IConfiguration _config;

        public AccountsController(IConfiguration config)
        {
            _config = config;
        }


        [HttpPost("Accounts/register")]
        public int PostUser([FromBody] User user)
        {
            //Check if user exists
            //Control 1 - Success, 0 -Unsucessful , -1 Error
            User existUser;

            int control = 2;
            existUser = connection.openConnection().QueryFirstOrDefault<User>(
                @"SELECT * FROM [PracDayDB].[dbo].[Users] WHERE UserName='" + user.UserName+"'");

            if(existUser == null)
            {
                try
                {
                    control = connection.openConnection().Execute(
                        @"INSERT INTO dbo.Users 
                        VALUES('"+user.UserName+"','"+user.Password+"')");
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

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] User login)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }


        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              null,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(User login)
        {
          
            //Validate the User Credentials
            User existUser = connection.openConnection().QueryFirstOrDefault<User>(
                @"SELECT * FROM dbo.Users WHERE UserName='"+login.UserName+"' AND Password='"+login.Password+"'");
            
            return existUser ;
        }

    }
}
