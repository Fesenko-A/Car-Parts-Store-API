using Common.Filters;
using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class ProductDAL {
        private readonly ApplicationDbContext _context;

        public ProductDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<(List<Product>, int)> GetAllProducts(ProductFilters filters) {
            IQueryable<Product> products = _context.Products.Include(p => p.Brand).Include(p => p.SpecialTag).Include(p => p.Category);

            if (!string.IsNullOrEmpty(filters.Brand) && filters.Brand != "All Brands") {
                products = products.Where(p => p.Brand.Name == filters.Brand);
            }

            if (!string.IsNullOrEmpty(filters.Category) && filters.Category != "All Categories") {
                products = products.Where(p => p.Category.Name == filters.Category);
            }

            if (!string.IsNullOrEmpty(filters.SpecialTag) && filters.SpecialTag != "All Special Tags") {
                products = products.Where(p => p.SpecialTag.Name == filters.SpecialTag);
            }

            if (!string.IsNullOrEmpty(filters.SearchString)) {
                products = products.Where(p => p.Name.ToLower().Contains(filters.SearchString.ToLower()));
            }

            if (!string.IsNullOrEmpty(filters.SortingOptions)) {
                if (filters.SortingOptions == SortingFilters.PRICE_LOW_HIGH) {
                    products = products.OrderBy(p => p.Price);
                }
                if (filters.SortingOptions == SortingFilters.PRICE_HIGH_LOW) {
                    products = products.OrderByDescending(p => p.Price);
                }
            }

            if (filters.OutOfStock == false) {
                products = products.Where(p => p.InStock == true);
            }

            int totalRecords = products.Count();

            products = products.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize);
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
