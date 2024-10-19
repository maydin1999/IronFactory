using Microsoft.AspNetCore.Mvc;

namespace IronFactoryAspNet.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        // Diğer kullanıcı bilgilerini buraya ekleyebilirsiniz.
    }
}
