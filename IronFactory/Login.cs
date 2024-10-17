using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronFactory
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var (connection, exception) = Helper.DatabaseConnector();
            
            if (exception != null)
            {
                MessageBox.Show("Veritabanı bağlantı hatası: " + exception.Message);
                return;
            }

            string username = txtUserName.Text;
            string password = txtPassword.Text;

            // Kullanıcı verisini al
            SqlCommand sqlCommand = new SqlCommand("SELECT EmployeePassword, salts, EmployeeName, EmployeeSurname FROM Employees WHERE EmployeeUserName=@username", connection);
            sqlCommand.Parameters.AddWithValue("@username", username);
            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                string storedHashedPassword = reader["EmployeePassword"].ToString();
                string salt = reader["salts"].ToString();
                string employeeName = reader["EmployeeName"].ToString();
                string employeeSurname = reader["EmployeeSurname"].ToString();
                //string employeeUsername = reader["EmployeeUserName"].ToString();

                // Şifreyi doğrula
                if (Helper.VerifyPassword(password, storedHashedPassword, salt))
                {
                    MessageBox.Show("Giriş Başarılı! Yönlendiriliyorsunuz", "Giriş Başarılı!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(2000); // Kullanıcı deneyimini iyileştirmek için süre
                    //Application.Run(new MainMenu(employeeName,employeeSurname));
                    MainMenu mainMenu = new MainMenu(employeeName, employeeSurname, username);
                    mainMenu.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Username or password wrong!");
                }
            }
            else
            {
                MessageBox.Show("Something went wrong!");
            }

            // Bağlantıyı kapat
            connection.Close();
        }
    }
}
