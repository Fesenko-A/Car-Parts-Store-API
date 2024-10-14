using BL.Models;
using BL.Services.Interfaces;
using DAL.Repository.Models;
using DAL.Services.Interfaces;

namespace BL.Services.Concrete {
    public class CategoryBL : ICategoryBL {
        private readonly IProductDetailsDAL<Category> _dal;

        public CategoryBL(IProductDetailsDAL<Category> dal) {
            _dal = dal;
        }

        public async Task<ErrorOr<Category>> Create(CategoryDto dto) {
            Category? categoryFromDb = await _dal.FindByName(dto.Name);

            if (categoryFromDb != null) {
                return new ErrorOr<Category>("Category already exists");
            }

            Category category = new Category {
                Name = dto.Name
            };

            await _dal.Create(category);
            categoryFromDb = await _dal.GetById(category.Id);

            if (categoryFromDb == null) {
                return new ErrorOr<Category>("Error while getting Category");
            }

            return new ErrorOr<Category>(categoryFromDb);
        }

        public async Task<List<Category>> GetAll() {
            List<Category> categories = await _dal.GetAll();
            return categories;
        }
    }
}
