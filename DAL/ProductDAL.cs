using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class ProductDAL {
        private readonly ApplicationDbContext _context;

        public ProductDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<(List<Product>, int)> GetAllProducts(string? brand, string? category, string? specialTag, string? searchString, int pageNumber, int pageSize) {
            IQueryable<Product> products = _context.Products.Include(p => p.Brand).Include(p => p.SpecialTag).Include(p => p.Category);

            if (!string.IsNullOrEmpty(brand) && brand != "All Brands") {
                products = products.Where(p => p.Brand.Name == brand);
            }

            if (!string.IsNullOrEmpty(category) && category != "All Categories") {
                products = products.Where(p => p.Category.Name == category);
            }

            if (!string.IsNullOrEmpty(specialTag) && specialTag != "All Special Tags") {
                products = products.Where(p => p.SpecialTag.Name == specialTag);
            }

            if (!string.IsNullOrEmpty(searchString)) {
                products = products.Where(p => p.Name.ToLower().Contains(searchString.ToLower()));
            }

            int totalRecords = products.Count();

            products = products.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var result = await products.ToListAsync();

            return (result, totalRecords);
        }

        public async Task<Product?> GetProduct(int id) {
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

        public async Task DeleteProduct(Product productToDelete) {
            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
