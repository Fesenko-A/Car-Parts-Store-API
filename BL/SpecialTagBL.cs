using BL.Interfaces;
using BL.Models;
using DAL.Repository.Models;
using DAL.Services.Concrete.EF;

namespace BL {
    public class SpecialTagBL : IProductDetailsBL<SpecialTag, SpecialTagDto> {
        private readonly SpecialTagDAL _dal;

        public SpecialTagBL () {
            _dal = new SpecialTagDAL();
        }

        public async Task<ErrorOr<SpecialTag>> Create(SpecialTagDto dto) {
            SpecialTag? specialTagFromDb = await _dal.FindByName(dto.Name);

            if (specialTagFromDb != null) {
                return new ErrorOr<SpecialTag>("Special tag already exists");
            }

            SpecialTag specialTag = new SpecialTag {
                Name = dto.Name
            };

            await _dal.Create(specialTag);
            specialTagFromDb = await _dal.GetById(specialTag.Id);

            if (specialTagFromDb == null) {
                return new ErrorOr<SpecialTag>("Error while getting Special tag");
            }

            return new ErrorOr<SpecialTag>(specialTagFromDb);
        }

        public async Task<List<SpecialTag>> GetAll() {
            List<SpecialTag> specialTags = await _dal.GetAll();
            return specialTags;
        }
    }
}
