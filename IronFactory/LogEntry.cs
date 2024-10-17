using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronFactory
{
    public class LogEntry
    {
        public int Id { get; set; } // Log kaydının benzersiz kimliği
        public DateTime Timestamp { get; set; } // Log zaman damgası
        public string Action { get; set; } // Yapılan işlem (örneğin: "Employee Updated")
        public string Details { get; set; } // Detaylar (değişen değerler, vb.)
        public string LogUser { get; set; } // İşlemi yapan kullanıcı
    }
}
