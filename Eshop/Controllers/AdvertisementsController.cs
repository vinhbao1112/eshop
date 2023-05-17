using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Areas.Admin.Models;
using Eshop.Data;

namespace Eshop.Controllers
{
    public class AdvertisementsController : Controller
    {
        private readonly EshopContext _context;

        public AdvertisementsController(EshopContext context)
        {
            _context = context;
        }

        // GET: Advertisements
        public async Task<IActionResult> Index()
        {
       
            var eshopContext = _context.Advertisement.Include(a => a.ImageType);
            return View(await eshopContext.ToListAsync());
        }

       
   
    }
}
