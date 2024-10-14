using BL.Models;
using Common.Filters;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface IProductBL {
        Task<(ErrorOr<List<Product>>, int)> GetAllProducts(ProductFilters filters);

        Task<ErrorOr<Product>> GetProduct(int id);

        Task<ErrorOr<Product>> CreateProduct(ProductDto productDto);

        Task<ErrorOr<Product>> UpdateProduct(int id, ProductDto productUpdateBody);

        Task<ErrorOr<bool>> DeleteProduct(int id);
    }
}
