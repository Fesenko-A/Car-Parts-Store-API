using BL.Models;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface ICategoryBL {
        Task<ErrorOr<Category>> Create(CategoryDto dto);

        Task<List<Category>> GetAll();
    }
}
