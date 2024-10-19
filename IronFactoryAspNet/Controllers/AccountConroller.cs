using Microsoft.AspNetCore.Mvc;
using IronFactoryAspNet.Data;
using IronFactoryAspNet.Models;
using IronFactory;

namespace IronFactoryAspNet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserRepository _userRepository;

        public AccountController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user != null && Helper.VerifyPassword(password, user.Password, user.Salt))
            {
                ViewBag.SuccessMessage = "Giriş başarılı!";
                return View();
            }

            // Giriş başarısız, hata mesajı göster
            ViewBag.ErrorMessage = "Geçersiz kullanıcı adı veya şifre.";
            return View();
        }
    }
}
