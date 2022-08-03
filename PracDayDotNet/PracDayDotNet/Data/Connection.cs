using System;
using System.Data.SqlClient;

namespace PracDay.Data
{
    public class Connection
    {
        private static readonly string _connectionString = "Data Source=DESKTOP-7SJDS25\\THABANGMSSQLSERV;Initial Catalog=PracDayDB;Integrated Security=True;TrustServerCertificate=True";
        internal SqlConnection _connect = new SqlConnection(_connectionString);

        public SqlConnection openConnection()
        {
            try
            {
                _connect.Open();
            }catch(Exception e)
            {
                e.GetBaseException();
            }
            return _connect;
        }

        public void closeConnection()
        {
            try
            {
                _connect.Close();

            }catch(Exception e)
            {
                e.GetBaseException();
            }
        }
    }
}
