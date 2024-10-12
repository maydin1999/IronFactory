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
    public partial class AddEmployee : Form
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

    }
}
