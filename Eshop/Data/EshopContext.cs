using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Eshop.Models;
using Eshop.Areas.Admin.Models;

namespace Eshop.Data
{
    public class EshopContext : DbContext
    {
        public EshopContext (DbContextOptions<EshopContext> options)
            : base(options)
        {
        }

        public DbSet<Eshop.Models.Account> Accounts { get; set; }
        public DbSet<Eshop.Models.ProductType> ProductTypes { get; set; }
        public DbSet<Eshop.Models.Product> Products { get; set; }
        public DbSet<Eshop.Models.Cart> Carts { get; set; }
        public DbSet<Eshop.Models.Invoice> Invoices { get; set; }
        public DbSet<Eshop.Models.InvoiceDetail> InvoiceDetails { get; set; }
        public DbSet<Eshop.Areas.Admin.Models.Comment> Comment { get; set; }
        public DbSet<Eshop.Areas.Admin.Models.Advertisement> Advertisement { get; set; }
    }
}
