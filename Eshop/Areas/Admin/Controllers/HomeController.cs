using Eshop.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly EshopContext _context;

        public HomeController(EshopContext context)
        {
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("username_admin") == null)
                return Redirect("Login");
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            int count = _context.Accounts.Count(acc => acc.Username == username && acc.Password == password && acc.IsAdmin == true);
            if (count == 1)
            {
                HttpContext.Session.SetString("username_admin", username);
                return View("Dashboard");
            }
            else
            {
                ViewBag.Msg = "Đăng nhập thất bại";
                return View();
            }
        }
    }
}
