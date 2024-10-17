using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronFactory
{
    public partial class AddEmployee : UserControl
    {
        public AddEmployee()
        {
            InitializeComponent();
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            var (connection, exception) = Helper.DatabaseConnector();

            if (connection != null)
            {
                try
                {
                    // Kullanımı güvenli bir şekilde yönetmek için using bloğu
                    using (connection)
                    {
                        SqlCommand sqlCommand = new SqlCommand("SELECT EmployeeTypeName FROM EmployeeTypes", connection);
                        using (SqlDataReader reader = sqlCommand.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // ComboBox'a EmployeeTypeName değerini ekle
                                cmbEmployeeType.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Query Error: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Connection Error: " + exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            connection.Close();
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            var (connection, exception) = Helper.DatabaseConnector();

            if (connection != null)
            {
                try
                {
                    // Kullanımı güvenli bir şekilde yönetmek için using bloğu
                    using (connection)
                    {
                        string password = txtPassword.Text;
                        string salt;

                        int SelectedCmb = cmbEmployeeType.SelectedIndex + 1;

                        string hashedPassword = Helper.HashPassword(password, out salt);
                        SqlCommand sqlCommand1 = new SqlCommand("INSERT INTO Employees (" +
                            "EmployeeUsername," +
                            "EmployeePassword," +
                            "EmployeeName," +
                            "EmployeeSurname," +
                            "EmployeePhoneNumber," +
                            "EmployeeMailAddress," +
                            "EmployeeAddress," +
                            "EmployeeTypeID," +
                            "EmployeeSalary," +
                            "EmployeeHolidayEntitlement," +
                            "salts) " +
                            "VALUES (@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10, @p11)", connection);
                        sqlCommand1.Parameters.AddWithValue("@p1", txtUserName.Text);
                        sqlCommand1.Parameters.AddWithValue("@p2", hashedPassword);
                        sqlCommand1.Parameters.AddWithValue("@p3", txtName.Text);
                        sqlCommand1.Parameters.AddWithValue("@p4", txtSurname.Text);
                        sqlCommand1.Parameters.AddWithValue("@p5", mskPhoneNumber.Text);
                        sqlCommand1.Parameters.AddWithValue("@p6", txtMailAddress.Text);
                        sqlCommand1.Parameters.AddWithValue("@p7", rchAddress.Text);
                        sqlCommand1.Parameters.AddWithValue("@p8", SelectedCmb);
                        sqlCommand1.Parameters.AddWithValue("@p9", txtSalary.Text);
                        sqlCommand1.Parameters.AddWithValue("@p10", txtHoliday.Text);
                        sqlCommand1.Parameters.AddWithValue("@p11", salt);
                        sqlCommand1.ExecuteNonQuery();
                        MessageBox.Show("Employee has been created!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Query Error: " + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Connection Error: " + exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            connection.Close();
        }
    }
}
