using DAL.Repository.Models;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;
using DAL.Services.Interfaces;

namespace DAL.Services.Concrete.EF
{
    public class SpecialTagDAL : IProductDetailsDAL<SpecialTag> {
        private readonly ApplicationDbContext _context;

        public SpecialTagDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<SpecialTag?> FindByName(string name) {
            SpecialTag? specialTag = await _context.SpecialTag.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefaultAsync();
            return specialTag;
        }

        public async Task<List<SpecialTag>> GetAll() {
            var specialTags = await _context.SpecialTag.ToListAsync();
            return specialTags;
        }

        public async Task<SpecialTag?> GetById(int id) {
            var specialTag = await _context.SpecialTag.Where(t => t.Id == id).FirstOrDefaultAsync();
            return specialTag;
        }

        public async Task Create(SpecialTag toAdd) {
            _context.SpecialTag.Add(toAdd);
            await _context.SaveChangesAsync();
        }
    }
}
