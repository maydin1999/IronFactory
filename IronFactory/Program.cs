using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace IronFactory
{
    public static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool developer_mode = true;

            var (connection, exception) = Helper.DatabaseConnector(); // Güncellenmiş kısım
            if (connection != null) // Bağlantı kontrolü
            {
                Application.Run(new Login());
                connection.Close(); // Uygulama kapandıktan sonra bağlantıyı kapat
            }
            else
            {
                if (developer_mode)
                {
                    Application.Run(new Developer());
                }

                MessageBox.Show("Connection Error: " + exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
    }
}
