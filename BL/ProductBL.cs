using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class ProductBL {
        private readonly DAL.ProductDAL _dal;

        public ProductBL()
        {
            _dal = new DAL.ProductDAL();
        }

        public async Task<List<Product>> GetAllProducts() {
            List<Product> productsFromDb = await _dal.GetAllProducts();
            return productsFromDb;
        }

        public async Task<Product> GetProduct(int id) {
            Product productFromDb = await _dal.GetProduct(id);
            return productFromDb;
        }

        public async Task<Product?> CreateProduct(ProductDto productDto) {
            Product product = new Product {
                BrandId = productDto.BrandId,
                Name = productDto.Name,
                Description = productDto.Description,
                SpecialTagId = productDto.SpecialTagId,
                CategoryId = productDto.CategoryId,
                Price = productDto.Price,
                ImageUrl = productDto.ImageUrl
            };

            try {
                await _dal.CreateProduct(product);
            } catch (DbUpdateException) {
                return null;
            }

            Product createdProduct = await GetProduct(product.Id);
            return createdProduct;
        }
    }
}
