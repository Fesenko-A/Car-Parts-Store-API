using BL.Models;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface ISpecialTagBL {
        Task<ErrorOr<SpecialTag>> Create(SpecialTagDto dto);

        Task<List<SpecialTag>> GetAll();
    }
}
