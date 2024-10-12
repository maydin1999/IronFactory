using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronFactory
{
    public partial class Developer : Form
    {
        public Developer()
        {
            InitializeComponent();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string databaseName = "IronFactory"; // Yedeklemek istediğiniz veritabanının adı
            string backupFilePath = @"C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\Backup\IronFactory_Backup.bak"; // Yedek dosyasının yolu

            Exception exception;

            bool isBackupSuccessful = Helper.BackupDatabase(databaseName, backupFilePath, out exception);

            if (isBackupSuccessful)
            {
                MessageBox.Show("Veritabanı yedeği başarıyla alındı!");
            }
            else
            {
                MessageBox.Show("Yedekleme hatası: " + exception.Message);
            }
            
        }

        private void Developer_Load(object sender, EventArgs e)
        {
            var (connection, exception) = Helper.DatabaseConnector();

            if (connection != null && connection.State == ConnectionState.Open)
            {
                lblStatus.Text = "ON";
                lblStatus.ForeColor = Color.Green;
            }
            else
            {
                lblStatus.Text = "OFF";
                lblStatus.ForeColor = Color.Red;

                // Hata mesajını göstermek isteyebilirsiniz
                if (exception != null)
                {
                    MessageBox.Show("Connection Error: " + exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
