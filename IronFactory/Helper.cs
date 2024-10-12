using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronFactory
{
    public static class Helper
    {
        public static (SqlConnection,Exception exception) DatabaseConnector()
        {
            Exception exception = null;
            //SqlConnection sqlConnection = new SqlConnection("Data Source = dePayens\\SQLEXPRESS; Initial Catalog = OgrenciYonetim; Integrated Security = True");
            string connectionString = "Server=MUHAMMED\\SQLEXPRESS;Database=IronFactory;Trusted_Connection=True;";
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open(); // Bağlantıyı açıyoruz
                return (connection, null); // Bağlantı başarılı
            }
            catch (Exception ex)
            {
                return (null, ex); // Bağlantı hatası
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

        // Tuz oluşturma
        private static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; // 128 bit
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(saltBytes);
            }
            return Convert.ToBase64String(saltBytes);
        }

        // Şifre doğrulama
        public static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            // Tuz ile birleştir ve hash'le
            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = password + salt;
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                var computedHash = Convert.ToBase64String(hashBytes);
                return computedHash == hashedPassword;
            }
        }
    }
}
