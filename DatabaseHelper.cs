using MySql.Data.MySqlClient;

namespace CybersecurityChatbot
{
    public class DatabaseHelper
    {
        private string connectionString =
            "server=localhost;port=3306;database=CybersecurityDB;uid=root;pwd=Azah@1745;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }
    }
}