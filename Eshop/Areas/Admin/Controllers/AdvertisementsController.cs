using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Areas.Admin.Models;
using Eshop.Data;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Eshop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdvertisementsController : Controller
    {
        private readonly EshopContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdvertisementsController(EshopContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Advertisements
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("username_admin") == null)
                return RedirectToAction("Login", "Home");
            var eshopContext = _context.Advertisement.Include(a => a.ImageType);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Admin/Advertisements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisement
                .Include(a => a.ImageType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // GET: Admin/Advertisements/Create
        public IActionResult Create()
        {
            ViewData["ImageTypeId"] = new SelectList(_context.Set<ImageType>(), "Id", "ImageName");
            return View();
        }

        // POST: Admin/Advertisements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ImageTypeId,ImageFile")] Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(advertisement);
                await _context.SaveChangesAsync();
                if (advertisement.ImageFile != null)
                {
                    var fileName = advertisement.Id.ToString() + Path.GetExtension(advertisement.ImageFile.FileName);
                    var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "advertisement");
                    var filePath = Path.Combine(uploadPath, fileName);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        advertisement.ImageFile.CopyTo(fs);
                        fs.Flush();
                    }
                    advertisement.Image = fileName;
                    _context.Update(advertisement);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["ImageTypeId"] = new SelectList(_context.Set<ImageType>(), "Id", "ImageName", advertisement.ImageTypeId);
            return View(advertisement);
        }

        // GET: Admin/Advertisements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisement.FindAsync(id);
            if (advertisement == null)
            {
                return NotFound();
            }
            ViewData["ImageTypeId"] = new SelectList(_context.Set<ImageType>(), "Id", "ImageName", advertisement.ImageTypeId);
            return View(advertisement);
        }

        // POST: Admin/Advertisements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Image,ImageTypeId,ImageFile")] Advertisement advertisement)
        {
            if (id != advertisement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(advertisement);
                    //Xóa ảnh cũ
                    if (advertisement.ImageFile != null)
                    {
                        var fileToDelete = Path.Combine(_webHostEnvironment.WebRootPath, "img", "advertisement", advertisement.Image);
                        FileInfo file = new FileInfo(fileToDelete);
                        file.Delete();
                    }
                    //Upload ảnh
                    if (advertisement.ImageFile != null)
                    {
                        var fileName = advertisement.Id.ToString() + Path.GetExtension(advertisement.ImageFile.FileName);
                        var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "img", "advertisement");
                        var filePath = Path.Combine(uploadPath, fileName);
                        using (FileStream fs = System.IO.File.Create(filePath))
                        {
                            advertisement.ImageFile.CopyTo(fs);
                            fs.Flush();
                        }
                        advertisement.Image = fileName;
                        _context.Update(advertisement);
                        await _context.SaveChangesAsync();
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdvertisementExists(advertisement.Id))
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
            ViewData["ImageTypeId"] = new SelectList(_context.Set<ImageType>(), "Id", "ImageName", advertisement.ImageTypeId);
            return View(advertisement);
        }

        // GET: Admin/Advertisements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var advertisement = await _context.Advertisement
                .Include(a => a.ImageType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (advertisement == null)
            {
                return NotFound();
            }

            return View(advertisement);
        }

        // POST: Admin/Advertisements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var advertisement = await _context.Advertisement.FindAsync(id);
            _context.Advertisement.Remove(advertisement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdvertisementExists(int id)
        {
            return _context.Advertisement.Any(e => e.Id == id);
        }
    }
}
