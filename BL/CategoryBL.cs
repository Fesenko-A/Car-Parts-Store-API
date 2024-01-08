using BL.Interfaces;
using BL.Models;
using DAL;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class CategoryBL : IProductDetailsBL<Category, CategoryDto> {
        private readonly DAL.CategoryDAL _dal;

        public CategoryBL() {
            _dal = new DAL.CategoryDAL();
        }

        public async Task<Category?> Create(CategoryDto dto) {
            Category? categoryFromDb = await _dal.FindCategoryByName(dto.Name);

            if (categoryFromDb == null) {
                Category category = new Category {
                    Name = dto.Name
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

        public async Task<List<Category>> GetAll() {
            List<Category> categories = await _dal.GetAllCategories();
            return categories;
        }
    }
}
