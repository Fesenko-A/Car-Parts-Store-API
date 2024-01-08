using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class SpecialTagBL {
        private readonly DAL.SpecialTagDAL _dal;

        public SpecialTagBL () {
            _dal = new DAL.SpecialTagDAL();
        }

        public async Task<SpecialTag?> CreateSpecialTag(SpecialTagDto specialTagDto) {
            SpecialTag? specialTagFromDb = await _dal.FindSpecialTagByName(specialTagDto.Name);

            if (specialTagFromDb == null) {
                SpecialTag specialTag = new SpecialTag {
                    Name = specialTagDto.Name
                };

                try {
                    await _dal.CreateSpecialTag(specialTag);
                    return await _dal.GetSpecialTag(specialTag.Id);
                }
                catch (DbUpdateException) {
                    return null;
                }
            }

            return specialTagFromDb;
        }

        public async Task<List<SpecialTag>> GetAllSpecialTags() {
            List<SpecialTag> categories = await _dal.GetAllSpecialTags();
            return categories;
        }
    }
}
