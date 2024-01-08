using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class BrandDAL {
        private readonly ApplicationDbContext _context;

        public BrandDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<Brand?> FindBrandByName(string brandName) {
            Brand? brand = await _context.Brand.Where(x => x.Name.ToLower() == brandName.ToLower()).FirstOrDefaultAsync();
            return brand;
        }

        public async Task<List<Brand>> GetAllBrands() {
            var brands = await _context.Brand.ToListAsync();
            return brands;
        }

        public async Task<Brand?> GetBrand(int id) {
            var brand = await _context.Brand.Where(b => b.Id == id).FirstOrDefaultAsync();
            return brand;
        }


        public async Task CreateBrand(Brand brandToAdd) {
            _context.Brand.Add(brandToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
