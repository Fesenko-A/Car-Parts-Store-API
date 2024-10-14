using BL.Models;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface IBrandBL {
        Task<ErrorOr<Brand>> Create(BrandDto dto);

        Task<List<Brand>> GetAll();
    }
}
