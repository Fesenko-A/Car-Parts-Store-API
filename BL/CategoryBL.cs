using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class CategoryBL {
        private readonly DAL.CategoryDAL _dal;

        public CategoryBL() {
            _dal = new DAL.CategoryDAL();
        }

        public async Task<Category?> CreateCategory(CategoryDto categoryDto) {
            Category? categoryFromDb = await _dal.FindCategoryByName(categoryDto.Name);

            if (categoryFromDb == null) {
                Category category = new Category {
                    Name = categoryDto.Name
                };

                try {
                    await _dal.CreateCategory(category);
                    return await _dal.GetCategory(category.Id);
                }
                catch (DbUpdateException) {
                    return null;
                }
            }

            return categoryFromDb;
        }

        public async Task<List<Category>> GetAllCategories() {
            List<Category> categories = await _dal.GetAllCategories();
            return categories;
        }
    }
}
