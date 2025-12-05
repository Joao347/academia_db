using MySql.Data.MySqlClient;
using System.Data;

namespace Academia.Data
{
    public class DatabaseConnection
    {
        private readonly string _connectionString;

        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public static string GetDefaultConnectionString()
        {
            return "Server=localhost;Database=academia_db;Uid=root;Pwd=root;";
        }
    }
}



