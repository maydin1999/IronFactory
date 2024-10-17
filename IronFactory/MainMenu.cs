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
        private string user;
        private string _employeeSurname;
        public MainMenu(string name, string surname, string username)
        {
            InitializeComponent();
            user = name;
            _employeeSurname = surname;
            lblName.Text = user;
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

                        // DataGridView için CellValueChanged olayını ekle
                        dataGridViewEmployees.CellValueChanged += (s, args) =>
                        {
                            if (dataGridViewEmployees.IsCurrentCellDirty)
                            {
                                connection.Open();
                                dataGridViewEmployees.CommitEdit(DataGridViewDataErrorContexts.Commit); // Değişiklikleri uygula

                                // Güncellenen hücreyi bul
                                int rowIndex = dataGridViewEmployees.CurrentCell.RowIndex;
                                int columnIndex = dataGridViewEmployees.CurrentCell.ColumnIndex;

                                // Güncellenen hücre verisi
                                var updatedValue = dataGridViewEmployees.CurrentCell.Value;

                                // Veritabanındaki ilgili kaydı güncelle
                                string employeeId = dataTable.Rows[rowIndex]["EmployeeID"].ToString(); // EmployeeID'yi al
                                string columnName = dataGridViewEmployees.Columns[columnIndex].Name; // Güncellenen sütun adı

                                string updateQuery = $"UPDATE Employees SET {columnName} = @value WHERE EmployeeID = @employeeId";

                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, connection))
                                {
                                    updateCommand.Parameters.AddWithValue("@value", updatedValue);
                                    updateCommand.Parameters.AddWithValue("@employeeId", employeeId);

                                    try
                                    {
                                        updateCommand.ExecuteNonQuery(); // Güncellemeyi uygula

                                        // Log kaydını ekle
                                        string action = "Employee Updated";
                                        string details = $"EmployeeID: {employeeId}, Column: {columnName}, New Value: {updatedValue}";
                                        Helper.LogAction(action, details, user); // Logla

                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show("Güncelleme hatası: " + ex.Message, "Hata!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    }
                                }
                                connection.Close();
                            }
                        };

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
