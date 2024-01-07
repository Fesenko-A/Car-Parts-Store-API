using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class ProductDAL {
        private readonly ApplicationDbContext _context;

        public ProductDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<List<Product>> GetAllProducts() {
            var products = await _context.Products.Include(p => p.Brand).Include(p => p.SpecialTag).Include(p => p.Category).ToListAsync();
            return products;
        }

        public async Task<Product> GetProduct(int id) {
            var product = await _context.Products.Include(p => p.Brand).Include(p => p.SpecialTag).Include(p => p.Category).Where(p => p.Id == id).FirstOrDefaultAsync();
            return product;
        }

        public async Task CreateProduct(Product product) {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProduct(Product productUpdate) {
            _context.Products.Update(productUpdate);
            await _context.SaveChangesAsync();
        }
    }
}
