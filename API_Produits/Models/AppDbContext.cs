using Microsoft.EntityFrameworkCore;
using System;

namespace API_Produits.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Café Noir", CategoryType = "Moulu" },
                new Category { CategoryId = 2, CategoryName = "Café Vert", CategoryType = "Grain" },
                new Category { CategoryId = 3, CategoryName = "Café Bleu", CategoryType = "Décafeiné" }
            );

            modelBuilder.Entity<Supplier>().HasData(
                new Supplier { SupplierId = 1, SupplierName = "Fournisseur de Café A", ContactEmail = "contactA@example.com" },
                new Supplier { SupplierId = 2, SupplierName = "Fournisseur de Café B", ContactEmail = "contactB@example.com" }
            );

            var utcNow = DateTime.UtcNow;

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    ProductId = 1,
                    Name = "Café Noir Intense",
                    Description = "Café noir à la saveur intense.",
                    Price = 3.50m,
                    StockQuantity = 150,
                    CategoryId = 1,
                    SupplierId = 1,
                    CreatedAt = utcNow,
                    UpdatedAt = utcNow
                },
                new Product
                {
                    ProductId = 2,
                    Name = "Café Vert Doux",
                    Description = "Café vert au goût doux et subtil.",
                    Price = 4.00m,
                    StockQuantity = 200,
                    CategoryId = 2,
                    SupplierId = 2,
                    CreatedAt = utcNow,
                    UpdatedAt = utcNow
                },
                new Product
                {
                    ProductId = 3,
                    Name = "Café Bleu Fruité",
                    Description = "Café bleu avec des notes fruitées.",
                    Price = 4.50m,
                    StockQuantity = 100,
                    CategoryId = 3,
                    SupplierId = 1,
                    CreatedAt = utcNow,
                    UpdatedAt = utcNow
                }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}