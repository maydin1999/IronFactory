using Microsoft.Data.SqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Data.SqlClient; // Güncellenmiş using direktifi
using Microsoft.Extensions.DependencyInjection; // Ensure this is included
using Microsoft.Extensions.Hosting; // Ensure this is included

namespace IronFactoryAspNet
{
    public static class AspHelper
    {

        public static (SqlConnection, Exception) DatabaseConnector(IConfiguration configuration)
        {
            Exception exception = null;
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open(); // Open the connection
                return (connection, null); // Connection successful
            }
            catch (Exception ex)
            {
                return (null, ex); // Connection error
            }
        }


        public static string HashPassword(string password, out string salt)
        {
            // Tuz oluştur
            salt = GenerateSalt();
            // Tuz ile birleştir ve hash'le
            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = password + salt;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                return Convert.ToBase64String(hashBytes);
            }
        }

        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; // 128 bit
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        public static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = password + salt;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                var computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == hashedPassword;
            }
        }

        public static bool BackupDatabase(string databaseName, string backupFilePath, out Exception exception)
        {
            exception = null;
            string connectionString = "Server=MUHAMMED\\SQLEXPRESS;Database=master;Trusted_Connection=True;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFilePath}'";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    return false;
                }
            }
        }

        public static async Task<(SqlConnection, Exception)> DatabaseConnectorAsync() // Change method signature
        {
            Exception exception = null;

            string connectionString = "Server=MUHAMMED\\SQLEXPRESS;Database=IronFactory;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync(); // Use OpenAsync
                return (connection, null);
            }
            catch (Exception ex)
            {
                return (null, ex);
            }
        }

        public static void LogAction(string action, string details, string user)
        {
            var (connection, exception) = DatabaseConnector(IConfiguration configuration);

            if (connection != null)
            {
                try
                {
                    string insertQuery = "INSERT INTO Logs (Timestamp, Action, Details, LogUser) VALUES (@timestamp, @action, @details, @user)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@timestamp", DateTime.Now);
                        command.Parameters.AddWithValue("@action", action);
                        command.Parameters.AddWithValue("@details", details);
                        command.Parameters.AddWithValue("@user", user);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Loglama hatası: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                Console.WriteLine("Bağlantı hatası: " + exception?.Message);
            }
        }
    }
}
