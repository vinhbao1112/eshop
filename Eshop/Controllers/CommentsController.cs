using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Areas.Admin.Models;
using Eshop.Data;
using Microsoft.AspNetCore.Http;

namespace Eshop.Controllers
{
    public class CommentsController : Controller
    {
        private readonly EshopContext _context;

        public CommentsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var eshopContext = _context.Comment.Include(c => c.Account).Include(c => c.Product);
            return View(await eshopContext.ToListAsync());
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AccountId,Content,CreatedAt,ProductId")] Comment comment)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                string a = "Đăng nhập má ơi!";
                return RedirectToAction("Index", "Home", new { name = a });
            }
            comment.CreatedAt = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Products", new { id = comment.ProductId });
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", comment.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", comment.ProductId);
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comment.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", comment.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", comment.ProductId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AccountId,Content,CreatedAt,ProductId")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.Id))
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
            ViewData["AccountId"] = new SelectList(_context.Accounts, "Id", "Password", comment.AccountId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", comment.ProductId);
            return View(comment);
        }

        // GET: Comments/Delete/5
     
        private bool CommentExists(int id)
        {
            return _context.Comment.Any(e => e.Id == id);
        }
    }
}
