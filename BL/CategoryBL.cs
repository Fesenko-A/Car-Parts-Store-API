using BL.Interfaces;
using BL.Models;
using DAL.Repository.Models;

namespace BL {
    public class CategoryBL : IProductDetailsBL<Category, CategoryDto> {
        private readonly DAL.CategoryDAL _dal;

        public CategoryBL() {
            _dal = new DAL.CategoryDAL();
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
