using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace PracDayDotNet.Data
{
    public class TokenClass
    {
        private static string _tokenString { set; get; }

        public void setTokenString(string tokenString)
        {
            _tokenString = tokenString;
        }


        public string getAuthorId()
        {
            //Decypher JWT
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(_tokenString);
            string authorId = decodedValue.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            return authorId;
        }

    }
}
