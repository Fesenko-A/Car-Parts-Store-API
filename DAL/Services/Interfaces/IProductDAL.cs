using Common.Filters;
using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    internal interface IProductDAL {
        Task<(List<Product>, int)> GetAllProducts(ProductFilters filters);

        Task<Product?> GetProduct(int id);

        Task CreateProduct(Product product);

        Task UpdateProduct(Product productUpdate);

        Task DeleteProduct(Product productToDelete);
    }
}
