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

        public async Task<Product?> GetProduct(int id) {
            Product? productFromDb = await _dal.GetProduct(id);
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

            Product createdProduct = await _dal.GetProduct(product.Id);
            return createdProduct;
        }

        public async Task<Product?> UpdateProduct(int id, ProductDto productUpdateBody) {
            Product? productToUpdate = await _dal.GetProduct(id);

            if (productToUpdate == null) {
                return null;
            }

            productToUpdate.BrandId = productUpdateBody.BrandId;
            productToUpdate.Name = productUpdateBody.Name;
            productToUpdate.Description = productUpdateBody.Description;
            productToUpdate.SpecialTagId = productUpdateBody.SpecialTagId;
            productToUpdate.CategoryId = productUpdateBody.CategoryId;
            productToUpdate.Price = productUpdateBody.Price;
            productToUpdate.ImageUrl = productUpdateBody.ImageUrl;

            try {
                await _dal.UpdateProduct(productToUpdate);
            } catch (DbUpdateException) { 
                return null; 
            }

            Product updatedProduct = await _dal.GetProduct(id);
            return updatedProduct;
        }

        public async Task<bool> DeleteProduct(int id) {
            Product? productToDelete = await _dal.GetProduct(id);

            if (productToDelete == null) { 
                return false; 
            }

            await _dal.DeleteProduct(productToDelete);
            return true;
        }
    }
}
