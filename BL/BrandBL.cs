using BL.Interfaces;
using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class BrandBL : IProductDetailsBL<Brand, BrandDto> {
        private readonly DAL.BrandDAL _dal;

        public BrandBL() { 
            _dal = new DAL.BrandDAL();
        }

        public async Task<Brand?> Create(BrandDto dto) {
            Brand? brandFromDb = await _dal.FindByName(dto.Name);

            if (brandFromDb == null) {
                Brand brand = new Brand {
                    Name = dto.Name
                };

                try {
                    await _dal.Create(brand);
                    return await _dal.GetById(brand.Id);
                }
                catch (DbUpdateException) {
                    return null;
                }
            }

            return brandFromDb;
        }

        public async Task<List<Brand>> GetAll() {
            List<Brand> brands = await _dal.GetAll();
            return brands;
        }
    }
}
