using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class BrandBL {
        private readonly DAL.BrandDAL _dal;

        public BrandBL() { 
            _dal = new DAL.BrandDAL();
        }

        public async Task<Brand?> CreateBrand(BrandDto brandDto) {
            Brand? brandFromDb = await _dal.FindBrandByName(brandDto.Name);

            if (brandFromDb == null) {
                Brand brand = new Brand {
                    Name = brandDto.Name
                };

                try {
                    await _dal.CreateBrand(brand);
                    return await _dal.GetBrand(brand.Id);
                }
                catch (DbUpdateException) {
                    return null;
                }
            }

            return brandFromDb;
        }

        public async Task<List<Brand>> GetAllBrands() {
            List<Brand> brands = await _dal.GetAllBrands();
            return brands;
        }
    }
}
