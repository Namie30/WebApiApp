using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllProducts() => await _context.Products.ToListAsync();

        public async Task<Product> GetProductById(int id) => await _context.Products.FindAsync(id);

        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetProductsByCategoryId(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalPriceByCategoryId(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId)
                .SumAsync(p => p.Price);
        }

        public async Task<Dictionary<string, decimal>> GetTotalPricePerCategory()
        {
            return await _context.Products
                .GroupBy(p => p.Category.Name)
                .Select(g => new { Category = g.Key, TotalPrice = g.Sum(p => p.Price) })
                .ToDictionaryAsync(x => x.Category, x => x.TotalPrice);
        }
    }
}
