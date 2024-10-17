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
using System.Xml.Linq;

namespace IronFactory
{
    public partial class MainMenu : Form
    {
        private string _employeeName;
        private string _employeeSurname;
        public MainMenu(string name, string surname, string username)
        {
            InitializeComponent();
            _employeeName = name;
            _employeeSurname = surname;
            lblName.Text = _employeeName;
            lblSurname.Text = _employeeSurname;
            // 1. TabControl'ü ve TabPage'i oluşturun
            // TabPage isimlerini değiştir
            tabMainMenu.SelectedIndexChanged += TabMainMenu_SelectedIndexChanged;

            tabMainMenu.TabPages[0].Text = "Add Employee";
            tabMainMenu.TabPages[1].Text = "Employee List";

        }



        private void TabMainMenu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Tab sayfası değiştiğinde kontrol et
            if (tabMainMenu.SelectedTab.Text == "Add Employee")
            {
                // AddEmployee formunu oluştur
                AddEmployee addEmployeeControl = new AddEmployee();

                // UserControl'ü tab içinde doldurun
                addEmployeeControl.Dock = DockStyle.Fill;
                tabMainMenu.SelectedTab.Controls.Clear(); // Önceki kontrolleri temizle
                tabMainMenu.SelectedTab.Controls.Add(addEmployeeControl); // Yeni UserControl ekle
                CenterControl(addEmployeeControl, tabMainMenu.SelectedTab);
            }
            else if (tabMainMenu.SelectedTab.Text == "Employee List")
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

                        // Önceki kontrolleri temizle
                        tabMainMenu.SelectedTab.Controls.Clear();
                        tabMainMenu.SelectedTab.Controls.Add(dataGridViewEmployees); // DataGridView'ı ekle
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

        private void CenterControl(Control control, Control parent)
        {
            control.Anchor = AnchorStyles.None; // Anchordan kaldır
            control.Location = new Point((parent.Width - control.Width) / 2, (parent.Height - control.Height) / 2);
        }
    }
}
