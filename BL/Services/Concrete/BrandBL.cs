using BL.Models;
using BL.Services.Interfaces;
using DAL.Repository.Models;
using DAL.Services.Interfaces;

namespace BL.Services.Concrete {
    public class BrandBL : IBrandBL {
        private readonly IProductDetailsDAL<Brand> _dal;

        public BrandBL(IProductDetailsDAL<Brand> dal) {
            _dal = dal;
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
