using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Produits.Controllers;
using API_Produits.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace API_Produits.Tests.UnitTests
{
    public class ProductsControllerTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public ProductsControllerTests()
        {
            // Configuration du contexte de base de données en mémoire pour les tests
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var category1 = new Category { CategoryId = 1, CategoryName = "Category 1", CategoryType = "Type 1" };
                var category2 = new Category { CategoryId = 2, CategoryName = "Category 2", CategoryType = "Type 2" };
                var supplier1 = new Supplier { SupplierId = 1, SupplierName = "Supplier 1", ContactEmail = "supplier1@example.com" };
                var supplier2 = new Supplier { SupplierId = 2, SupplierName = "Supplier 2", ContactEmail = "supplier2@example.com" };

                context.Products.AddRange(
                    new Product
                    {
                        ProductId = 1,
                        Name = "Product 1",
                        Description = "Description for Product 1",
                        Price = 10.50m,
                        StockQuantity = 100,
                        CategoryId = category1.CategoryId,
                        Category = category1,
                        SupplierId = supplier1.SupplierId,
                        Supplier = supplier1,
                        CreatedAt = DateTime.Now.AddDays(-10),
                        UpdatedAt = DateTime.Now.AddDays(-5)
                    },
                    new Product
                    {
                        ProductId = 2,
                        Name = "Product 2",
                        Description = "Description for Product 2",
                        Price = 20.75m,
                        StockQuantity = 150,
                        CategoryId = category2.CategoryId,
                        Category = category2,
                        SupplierId = supplier2.SupplierId,
                        Supplier = supplier2,
                        CreatedAt = DateTime.Now.AddDays(-15),
                        UpdatedAt = DateTime.Now.AddDays(-8)
                    }
                );

                context.SaveChanges();
            }
        }

        [Fact]
        public async Task GetProducts_ReturnsAllProducts()
        {
            // Arrange
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);

                // Act
                var result = await controller.GetProducts();

                // Assert
                var actionResult = Assert.IsType<ActionResult<IEnumerable<Product>>>(result);
                var products = Assert.IsAssignableFrom<IEnumerable<Product>>(actionResult.Value);
                Assert.Equal(2, products.Count());
            }
        }

        // Ajouter d'autres tests au besoin
    }
}
