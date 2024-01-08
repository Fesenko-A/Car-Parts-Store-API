using DAL.Repository.Models;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;

namespace DAL {
    public class CategoryDAL : IProductDetailsDAL<Category> {
        private readonly ApplicationDbContext _context;

        public CategoryDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<Category?> FindByName(string name) {
            Category? category = await _context.Category.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
            return category;
        }

        public async Task<List<Category>> GetAll() {
            var categories = await _context.Category.ToListAsync();
            return categories;
        }

        public async Task<Category?> GetById(int id) {
            var category = await _context.Category.Where(c => c.Id == id).FirstOrDefaultAsync();
            return category;
        }

        public async Task Create(Category toAdd) {
            _context.Category.Add(toAdd);
            await _context.SaveChangesAsync();
        }
    }
}
