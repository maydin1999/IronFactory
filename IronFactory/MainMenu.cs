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
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Linq;

namespace IronFactory
{
    public partial class MainMenu : Form
    {
        private string user;
        private string _employeeSurname;
        public MainMenu(string name, string surname, string username)
        {
            InitializeComponent();
            user = name;
            _employeeSurname = surname;
            lblName.Text = user;
            lblSurname.Text = _employeeSurname;
        }

        private void addEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmployee addEmployee = new AddEmployee();
            addEmployee.Show();
        }

        private void showEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var (connection, exception) = Helper.DatabaseConnector();

            if (connection != null)
            {
                try
                {
                    // DataGridView oluştur
                    DataGridView dataGridViewEmployees = new DataGridView();

                    // SQL komutunu oluştur
                    SqlCommand sqlCommand = new SqlCommand("SELECT * FROM Employees", connection);
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(sqlCommand);
                    DataTable dataTable = new DataTable();

                    // Verileri doldur
                    dataAdapter.Fill(dataTable);
                    dataGridViewEmployees.DataSource = dataTable;

                    // DataGridView'ı tam ekran yap
                    dataGridViewEmployees.Dock = DockStyle.Fill; // DataGridView'ı forma yay
                    panel1.Controls.Add(dataGridViewEmployees);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Veri alma hatası: " + ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    // Bağlantıyı kapat
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Bağlantı hatası: " + exception.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
