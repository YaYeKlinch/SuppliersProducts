using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SuppliersProducts.Models;

namespace SuppliersProducts.Data
{
    public class DataBaseContext:DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Buyer> Buyers { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Buyer>().ToTable("Buyer");
            modelBuilder.Entity<Order>().ToTable("Order");
        }

        public DbSet<SuppliersProducts.Models.Supplier> Supplier { get; set; }

        public DbSet<SuppliersProducts.Models.SupplierProduct> SupplierProduct { get; set; }
    }

}
