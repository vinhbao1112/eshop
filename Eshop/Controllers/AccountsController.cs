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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Eshop.Controllers
{
    public class AccountsController : Controller
    {
        private readonly EshopContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountsController(EshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details()
        {
            string username = HttpContext.Session.GetString("username");
            var account = await _context.Accounts
               .FirstOrDefaultAsync(m => m.Username == username);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
            return View(await _context.Accounts.ToListAsync());
        }

        // GET: Accounts/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,ImageFile,Status")] Account account)
        {
            if (ModelState.IsValid)
            {
                _context.Add(account);
                await _context.SaveChangesAsync();


                if (account.ImageFile != null)
                {
                    var fileName = account.Id.ToString() + Path.GetExtension(account.ImageFile.FileName);
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "avatar");
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        account.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    account.Avatar = fileName;
                    _context.Update(account);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction("Index","Home");
            }
            return View(account);
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Login(string Username,  string Password)
        {
            int count = _context.Accounts.Count(acc => acc.Username == Username && acc.Password == Password && acc.Status == true);
            var account = _context.Accounts.FirstOrDefault(a => a.Username == Username && a.Password == Password);
                if(count == 1)
            {
                HttpContext.Session.SetString("username", Username);
                HttpContext.Session.SetInt32("Id_Cart", account.Id);
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.Msg = "Đăng nhập thất bại";
                return View();
            }
        }
        // GET: Admin/Accounts/Edit/5
        public async Task<IActionResult> Edit()
        {
            string username = HttpContext.Session.GetString("username");
            var account = await _context.Accounts
               .FirstOrDefaultAsync(m => m.Username == username);
            if (account == null)
            {
                return RedirectToAction("Index","Home");
            }
            
            ViewBag.Invoice = _context.Invoices.Include(a => a.Account).Where(c => c.AccountId == account.Id);
            ViewBag.username = HttpContext.Session.GetString("username");
            return View(account);
            return View(await _context.Accounts.ToListAsync());
        }

        // POST: Admin/Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Email,Phone,Address,FullName,IsAdmin,Avatar,ImageFile,Status")] Account account)
        {
            if (id != account.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    //Xóa ảnh cũ
                    if (account.ImageFile != null)
                    {
                        var fileToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "img", "avatar", account.Avatar);
                        FileInfo file = new FileInfo(fileToDelete);
                        file.Delete();
                    }
                    //Upload ảnh
                    if (account.ImageFile != null)
                    {
                        var fileName = account.Id.ToString() + Path.GetExtension(account.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "avatar");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            account.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        account.Avatar = fileName;
                        _context.Update(account);
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                HttpContext.Session.SetString("username", account.Username);
                return RedirectToAction("Index", "Home");
            }
            return View(account);
        }
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }

    }
}
