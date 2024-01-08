using DAL.Repository.Models;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class CategoryDAL {
        private readonly ApplicationDbContext _context;

        public CategoryDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<Category?> FindCategoryByName(string categoryName) {
            Category? category = await _context.Category.Where(x => x.Name.ToLower() == categoryName.ToLower()).FirstOrDefaultAsync();
            return category;
        }

        public async Task<List<Category>> GetAllCategories() {
            var categories = await _context.Category.ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategory(int id) {
            var category = await _context.Category.Where(b => b.Id == id).FirstOrDefaultAsync();
            return category;
        }


        public async Task CreateCategory(Category categoryToAdd) {
            _context.Category.Add(categoryToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
