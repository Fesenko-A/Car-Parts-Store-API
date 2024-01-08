using DAL.Repository.Models;
using DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class SpecialTagDAL {
        private readonly ApplicationDbContext _context;

        public SpecialTagDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<SpecialTag?> FindSpecialTagByName(string specialTagName) {
            SpecialTag? specialTag = await _context.SpecialTag.Where(x => x.Name.ToLower() == specialTagName.ToLower()).FirstOrDefaultAsync();
            return specialTag;
        }

        public async Task<List<SpecialTag>> GetAllSpecialTags() {
            var specialTags = await _context.SpecialTag.ToListAsync();
            return specialTags;
        }

        public async Task<SpecialTag?> GetSpecialTag(int id) {
            var specialTag = await _context.SpecialTag.Where(t => t.Id == id).FirstOrDefaultAsync();
            return specialTag;
        }


        public async Task CreateSpecialTag(SpecialTag specialTagToAdd) {
            _context.SpecialTag.Add(specialTagToAdd);
            await _context.SaveChangesAsync();
        }
    }
}
