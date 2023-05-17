using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Data;
using Eshop.Models;
using Microsoft.AspNetCore.Http;

namespace Eshop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;

        public ProductsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
           
            ViewBag.idProduct = id;
            var account = _context.Accounts.FirstOrDefault(a => a.Username == HttpContext.Session.GetString("username"));
            if (account == null)
            {
                ViewBag.idAccount = null;
            }
            else { 
                ViewBag.idAccount = account.Id;
            }
            ViewBag.AccountComment = _context.Comment.Include(c=>c.Account).Where(a=>a.Account.Id  == a.AccountId).Where(a=> a.ProductId == id);
            ViewBag.SumComment = _context.Comment.Include(c => c.Account).Where(a => a.Account.Id == a.AccountId).Where(a => a.ProductId == id).Count();
            return View(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Abado(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.Author == "Abado");
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Hanhquan(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.Author == "Hành Quân");
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Rossie(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.Author == "Rossie");
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Johncena(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.Author == "John Ce na");
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Khanquangco(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.Publishing == "Khăn Choàng Cổ");
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Kimdongdoan(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.Publishing == "Kim Đồng Đoàn");
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Sachgiaokhoa(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.ProductTypeId == 1);
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Sachthamkhao(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.ProductTypeId == 2);
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Sachnuocngoai(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.ProductTypeId == 3);
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Bao(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.ProductTypeId == 4);
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Tieuthuyet(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.ProductTypeId == 5);
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
        public async Task<IActionResult> Khac(string name)
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            else
            {
                ViewBag.Err = name;

            }
            ViewBag.Banner = _context.Advertisement.Where(a => a.ImageTypeId == 2);
            ViewBag.Adv = _context.Advertisement.Where(a => a.ImageTypeId == 1);
            var eshopContext = _context.Products.Where(p => p.ProductTypeId == 6);
            return View(await eshopContext.OrderBy(x => x.Id).Take(12).ToListAsync());
        }
    }
}
