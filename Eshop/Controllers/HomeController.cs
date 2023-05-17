using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Eshop.Controllers
{
    public class HomeController : Controller
    {

        

        private readonly EshopContext _context;

        public HomeController(EshopContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string name)
        {
            if(HttpContext.Session.GetString("username") != null)
            {             
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a=>a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a=>a.ImageTypeId == 1);
            var eshopContext = _context.Products.Include(p => p.ProductType).Where(a=>a.Status==true);
            return View(await eshopContext.OrderBy(x=>x.Id).Take(12).ToListAsync());
        }


        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
