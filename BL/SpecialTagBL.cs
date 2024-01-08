using BL.Interfaces;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class SpecialTagBL : IProductDetailsBL<SpecialTag, SpecialTagDto> {
        private readonly DAL.SpecialTagDAL _dal;

        public SpecialTagBL () {
            _dal = new DAL.SpecialTagDAL();
        }

        public async Task<SpecialTag?> Create(SpecialTagDto dto) {
            SpecialTag? specialTagFromDb = await _dal.FindByName(dto.Name);

            if (specialTagFromDb == null) {
                SpecialTag specialTag = new SpecialTag {
                    Name = dto.Name
                };

                try {
                    await _dal.Create(specialTag);
                    return await _dal.GetById(specialTag.Id);
                }
                catch (DbUpdateException) {
                    return null;
                }
            }

            return specialTagFromDb;
        }

        public async Task<List<SpecialTag>> GetAll() {
            List<SpecialTag> categories = await _dal.GetAll();
            return categories;
        }
    }
}
