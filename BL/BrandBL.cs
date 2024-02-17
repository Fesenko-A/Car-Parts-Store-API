using BL.Interfaces;
using BL.Models;
using DAL.Repository.Models;

namespace BL {
    public class BrandBL : IProductDetailsBL<Brand, BrandDto> {
        private readonly DAL.BrandDAL _dal;

        public BrandBL() { 
            _dal = new DAL.BrandDAL();
        }

        public async Task<ErrorOr<Brand>> Create(BrandDto dto) {
            Brand? brandFromDb = await _dal.FindByName(dto.Name);

            if (brandFromDb != null) {
                return new ErrorOr<Brand>("Brand already exists");
            }

            Brand brand = new Brand {
                Name = dto.Name
            };

            await _dal.Create(brand);
            brandFromDb = await _dal.GetById(brand.Id);

            if (brandFromDb == null) {
                return new ErrorOr<Brand>("Error while getting Brand");
            }

            return new ErrorOr<Brand>(brandFromDb);
        }

        public async Task<List<Brand>> GetAll() {
            List<Brand> brands = await _dal.GetAll();
            return brands;
        }
    }
}
