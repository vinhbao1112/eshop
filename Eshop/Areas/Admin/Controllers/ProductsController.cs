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

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly EshopContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(EshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            //ViewData["productTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            if (HttpContext.Session.GetString("username_admin") == null)
                return RedirectToAction("Login", "Home");
            var eshopContext = _context.Products.Include(p => p.ProductType);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
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

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SKU,Name,Author,Price,Stock,ProductTypeId,ImageFile,Status,CreatedAt,Description,NumerOfPages,Publishing")] Product product)
        {
            product.CreatedAt = DateTime.Now;
           
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();


                if (product.ImageFile != null)
                {
                    if (product.ImageFile.Length <= 500 * 1024) //quy đổi kilobyte thành byte
                    {
                        
                            var fileName = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "product");
                            var filePath = Path.Combine(uploadPath, fileName);
                            using (FileStream fs = System.IO.File.Create(filePath))
                            {
                                product.ImageFile.CopyTo(fs);
                                fs.Flush();
                            }
                            product.Image = fileName;
                            _context.Update(product);
                            await _context.SaveChangesAsync();
                        
                       
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.total = _context.Products.Count(a => a.ProductTypeId == id);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SKU,Name,Description,Price,Stock,ProductTypeId,Image,ImageFile,Status,Author,NumerOfPages,Publishing")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    //Xóa ảnh cũ
                    if (product.ImageFile != null)
                    {
                        var fileToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "img", "product", product.Image);
                        FileInfo file = new FileInfo(fileToDelete);
                        file.Delete();
                    }
                    //Upload ảnh
                    if (product.ImageFile != null)
                    {
                        var fileName = product.Id.ToString() + Path.GetExtension(product.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "product");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            product.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        product.Image = fileName;
                        _context.Update(product);
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Id", product.ProductTypeId);
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public IActionResult Search(string sku="", string name="", string author="", int miniprice = 0, int maxprice = int.MaxValue)
        {
            ViewData["productTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            if (sku == null) sku = "";
            if (name == null) name = "";
            if (author == null) author = "";
            
            var products = _context.Products.Include(a=>a.ProductType)
                                            .Where(a => a.SKU.Contains(sku))
                                            .Where(a => a.Name.Contains(name))
                                            .Where(a => a.Author.Contains(author))
                                            //.Where(a=>a.ProductTypeId == productTypeId)
                                            .Where(a => a.Price >= miniprice && a.Price <= maxprice).ToList();
            return View("Index",products);
        }
    }
}
