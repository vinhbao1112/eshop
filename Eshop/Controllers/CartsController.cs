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
    public class CartsController : Controller
    {
        private readonly EshopContext _context;

        public CartsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("username") != null)
            {
                ViewBag.username = HttpContext.Session.GetString("username");
            }
            // Lấy ra giỏ hàng của tài khoản đang đăng nhập từ Session
            string username = HttpContext.Session.GetString("username");
            var eshopContext = _context.Carts.Include(c => c.Account).Include(c => c.Product)
                .Where(c=> c.Account.Username == username);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,ProductId,Quantity")] Cart cart)
        {
            if (id != cart.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Username", cart.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cart.ProductId);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Account)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }

        public IActionResult Pay()
        {
            string username = HttpContext.Session.GetString("username");

            ViewBag.Account = _context.Accounts.Where(a => a.Username == username).FirstOrDefault();

            ViewBag.InvoicesTotal = _context.Carts.Include(c => c.Account).Include(c => c.Product)
                .Where(c => c.Account.Username == username).Sum(c => c.Product.Price * c.Quantity);
            return View();
        }

        [HttpPost]
        public IActionResult Pay([Bind("ShippingAddress, ShippingPhone")] Invoice invoice)
        {
            string username = HttpContext.Session.GetString("username");

            if (!CheckStock(username))
            {
                ViewBag.ErrorMessage = "Có sản phẩm đã hết hàng.";
                invoice.AccountId = _context.Accounts.Where(a => a.Username == username).FirstOrDefault().Id;
                invoice.Total = _context.Carts.Include(c => c.Account).Include(c => c.Product)
                    .Where(c => c.Account.Username == username).Sum(c => c.Product.Price * c.Quantity);
                return View("Index");
            }
            // Them hoa don
            DateTime now = DateTime.Now;
            invoice.IssuedDate = now;
            invoice.Code = now.ToString("yyMMddhhmmss");
            invoice.AccountId = _context.Accounts.Where(a => a.Username == username).FirstOrDefault().Id;
            invoice.Total = _context.Carts.Include(c => c.Account).Include(c => c.Product)
                .Where(c => c.Account.Username == username).Sum(c => c.Product.Price * c.Quantity);
            _context.Add(invoice);
            _context.SaveChanges();

            // Them chi tiet hoa don
            List<Cart> carts = _context.Carts.Include(c => c.Account).Include(c => c.Product)
               .Where(c => c.Account.Username == username).ToList();
            foreach (Cart c in carts)
            {
                InvoiceDetail ind = new InvoiceDetail();
                ind.InvoiceId = invoice.Id;
                ind.ProductId = c.ProductId;
                ind.Quantity = c.Quantity;
                ind.UnitPrice = c.Product.Price;
                _context.Add(ind);
            }
            _context.SaveChanges();
            foreach (Cart c in carts)
            {
                c.Product.Stock -= c.Quantity;
                _context.Products.Update(c.Product);
                _context.Carts.Remove(c);
            }
            _context.SaveChanges();
                return RedirectToAction("Index","Home");
        }
        // check stock
         private bool CheckStock(string username)
        {
            List<Cart> carts = _context.Carts.Include(c => c.Product).Include(c => c.Account)
                .Where(c => c.Account.Username == username).ToList();
            foreach(Cart c in carts)
            {
                if(c.Product.Stock < c.Quantity)
                {
                    return false;
                }
            }
                return true;
        }

        //GET Add Carts/1
        public async Task<IActionResult> Add(int id, int quantity = 1)
        {
            if(HttpContext.Session.GetString("username") == null)
            {
                string a = "Đăng nhập má ơi!";
                return RedirectToAction("Index","Home", new { name =a}  );
            }
            int Id = (int)HttpContext.Session.GetInt32("Id_Cart");
            var carts = _context.Carts.FirstOrDefault(c => c.ProductId == id && c.AccountId == Id);
            if (carts == null)
            {

                Cart cart = new Cart()
                {
                    AccountId = Id,
                    ProductId = id,
                    Quantity = quantity,
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                carts.Quantity += quantity;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

    }
}
