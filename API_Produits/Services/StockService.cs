using API_Produits.Models;
using System.Threading.Tasks;

namespace API_Produits.Services
{
    public interface IStockService
{
    Task<bool> IsStockAvailable(int productId, int quantity);
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
        return product != null && product.StockQuantity >= quantity;
    }

    }
}


