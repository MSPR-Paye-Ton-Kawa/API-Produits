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

        // R�cup�rer tous les produits
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
        
        // Cr�er un nouveau produit
        [Fact]
        public async Task PostProduct_CreatesNewProduct()
        {
            var newProduct = new Product
            {
                Name = "Caf� �pic�",
                Description = "Un caf� avec un go�t �pic�.",
                Price = 5.99m,
                StockQuantity = 50,
                CategoryId = 1,
                SupplierId = 1,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);

                var result = await controller.PostProduct(newProduct);

                var actionResult = Assert.IsType<ActionResult<Product>>(result);
                var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
                var product = Assert.IsType<Product>(createdResult.Value);
                Assert.Equal(newProduct.Name, product.Name);
                Assert.Equal(newProduct.Description, product.Description);
            }
        }

        // R�cup�rer un produit existant
        [Fact]
        public async Task GetProduct_ReturnsProduct_WhenProductExists()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);
                var existingProductId = 1;
                var result = await controller.GetProduct(existingProductId);

                var actionResult = Assert.IsType<ActionResult<Product>>(result);
                var product = Assert.IsType<Product>(actionResult.Value);
                Assert.Equal(existingProductId, product.ProductId);
            }
        }

        // R�cup�rer un produit inexistant
        [Fact]
        public async Task GetProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);
                var nonExistingProductId = 999; // ID qui n'existe pas

                var result = await controller.GetProduct(nonExistingProductId);
                Assert.IsType<NotFoundResult>(result.Result);
            }
        }

        // Modifier un produit existant
        [Fact]
        public async Task PutProduct_UpdatesExistingProduct()
        {
            var updatedProduct = new Product
            {
                ProductId = 1,
                Name = "Caf� Noir Intense Modifi�",
                Description = "Caf� noir � la saveur intense et modifi�e.",
                Price = 3.8m,
                StockQuantity = 150,
                CategoryId = 1,
                SupplierId = 1,
                CreatedAt = DateTime.Now.AddDays(-10),
                UpdatedAt = DateTime.Now
            };

            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);

                var result = await controller.PutProduct(updatedProduct.ProductId, updatedProduct);

                Assert.IsType<NoContentResult>(result);
                var product = await context.Products.FindAsync(updatedProduct.ProductId);
                Assert.Equal(updatedProduct.Name, product.Name);
            }
        }
        
        // Modifier un produit inexistant
        [Fact]
        public async Task PutProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);
                var product = new Product
                {
                    ProductId = 999, 
                    Name = "Produit Inexistant",
                    Description = "Description",
                    Price = 10.0m,
                    StockQuantity = 10,
                    CategoryId = 1,
                    SupplierId = 1
                };

                var result = await controller.PutProduct(999, product);

                Assert.IsType<NotFoundResult>(result);
            }
        }

        // Supprimer un produit existant
        [Fact]
        public async Task DeleteProduct_RemovesProduct()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);
                var productIdToDelete = 1;

                var result = await controller.DeleteProduct(productIdToDelete);

                Assert.IsType<NoContentResult>(result);
                var product = await context.Products.FindAsync(productIdToDelete);
                Assert.Null(product);
            }
        }

        // Supprimer un produit inexistant
        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            using (var context = new AppDbContext(_dbContextOptions))
            {
                var controller = new ProductsController(context);

                var result = await controller.DeleteProduct(999);

                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}
