using API_Produits.Models;
using Serilog;

namespace API_Produits.Services
{
    public interface IStockService
    {
        Task<bool> IsStockAvailable(int productId, int quantity);
        Task<bool> UpdateStockQuantity(int productId, int quantity);
    }

public class StockService : IStockService
    {
        private readonly AppDbContext _context; 
        public StockService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsStockAvailable(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null)
            {
                Log.Information($"Product with ID {productId} not found.");
                return false; 
            }

            Log.Information($"Product found: ID = {productId}, StockQuantity = {product.StockQuantity}, RequestedQuantity = {quantity}");

            return product.StockQuantity >= quantity;
        }

        public async Task<bool> UpdateStockQuantity(int productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);

            if (product == null || product.StockQuantity < quantity)
            {
                return false; 
            }

            product.StockQuantity -= quantity;

            await _context.SaveChangesAsync();

            return true; 
        }

    }
}


