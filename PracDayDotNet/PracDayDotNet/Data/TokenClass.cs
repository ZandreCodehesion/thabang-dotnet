namespace PracDayDotNet.Data
{
    public class TokenClass
    {
        private static string _tokenString { set; get; }

        public void setTokenString(string tokenString)
        {
            _tokenString = tokenString;
        }

        public string getToken()
        {
            return _tokenString;
        }
    }
}
