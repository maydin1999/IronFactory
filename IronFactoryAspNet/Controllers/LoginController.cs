using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using IronFactoryAspNet; // Burayı güncelledik

namespace IronFactoryAspNet.Controllers
{
    [Route("Login")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration) // Inject IConfiguration
        {
            _configuration = configuration;
        }
        [HttpPost("LoginUser")]
        public async Task<IActionResult> LoginUser(string username, string password)
        {
            var (connection, exception) = await AspHelper.DatabaseConnectorAsync(); // Updated call

            if (exception != null)
            {
                return BadRequest("Veritabanı bağlantı hatası: " + exception.Message);
            }

            // Kullanıcı verisini al
            SqlCommand sqlCommand = new SqlCommand("SELECT EmployeePassword, salts, EmployeeName, EmployeeSurname FROM Employees WHERE EmployeeUserName=@username", connection);
            sqlCommand.Parameters.AddWithValue("@username", username);
            SqlDataReader reader = await sqlCommand.ExecuteReaderAsync();

            if (reader.Read())
            {
                string storedHashedPassword = reader["EmployeePassword"].ToString();
                string salt = reader["salts"].ToString();
                string employeeName = reader["EmployeeName"].ToString();
                string employeeSurname = reader["EmployeeSurname"].ToString();

                // Şifreyi doğrula
                if (AspHelper.VerifyPassword(password, storedHashedPassword, salt)) // Corrected from Helper to AspHelper
                {
                    // Giriş başarılı, yönlendirme yapılacak
                    // Örneğin, ana menü sayfasına yönlendirme yapılabilir
                    return RedirectToAction("Index", "MainMenu", new { name = employeeName, surname = employeeSurname, username });
                }
                else
                {
                    return BadRequest("Kullanıcı adı veya şifre hatalı!");
                }
            }
            else
            {
                return BadRequest("Bir şeyler yanlış gitti!");
            }

            // Bağlantıyı kapat
            connection.Close();
        }
    }
}
