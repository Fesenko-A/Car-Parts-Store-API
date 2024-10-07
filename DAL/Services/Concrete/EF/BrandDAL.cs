using DAL.Repository;
using DAL.Repository.Models;
using DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.Concrete.EF
{
    public class BrandDAL : IProductDetailsDAL<Brand>
    {
        private readonly ApplicationDbContext _context;

        public BrandDAL()
        {
            _context = new ApplicationDbContext();
        }

        public async Task<Brand?> FindByName(string name)
        {
            Brand? brand = await _context.Brand.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
            return brand;
        }

        public async Task<List<Brand>> GetAll()
        {
            var brands = await _context.Brand.ToListAsync();
            return brands;
        }

        public async Task<Brand?> GetById(int id)
        {
            var brand = await _context.Brand.Where(b => b.Id == id).FirstOrDefaultAsync();
            return brand;
        }

        public async Task Create(Brand toAdd)
        {
            _context.Brand.Add(toAdd);
            await _context.SaveChangesAsync();
        }
    }
}
