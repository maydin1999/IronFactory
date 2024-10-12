using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronFactory
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var (connection, exception) = Helper.DatabaseConnector(); // Güncellenmiş kısım
            if (connection != null) // Bağlantı kontrolü
            {
                Application.Run(new Developer());
                connection.Close(); // Uygulama kapandıktan sonra bağlantıyı kapat
            }
            else
            {
                MessageBox.Show("Connection Error: " + exception.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
