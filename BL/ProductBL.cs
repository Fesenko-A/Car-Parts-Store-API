﻿using BL.Models;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class ProductBL {
        private readonly DAL.ProductDAL _dal;

        public ProductBL() {
            _dal = new DAL.ProductDAL();
        }

        public async Task<List<Product>> GetAllProducts() {
            List<Product> productsFromDb = await _dal.GetAllProducts();
            return productsFromDb;
        }

        public async Task<ErrorOr<Product>> GetProduct(int id) {
            Product? productFromDb = await _dal.GetProduct(id);
            
            if (productFromDb == null) {
                return new ErrorOr<Product>("Product not found");
            }

            return new ErrorOr<Product>(productFromDb);
        }

        public async Task<ErrorOr<Product>> CreateProduct(ProductDto productDto) {
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
                return new ErrorOr<Product>("Error while creating Product");
            }

            Product? createdProduct = await _dal.GetProduct(product.Id);

            if (createdProduct == null) {
                return new ErrorOr<Product>("Error while getting Product");
            }

            return new ErrorOr<Product>(createdProduct);
        }

        public async Task<ErrorOr<Product>> UpdateProduct(int id, ProductDto productUpdateBody) {
            Product? productToUpdate = await _dal.GetProduct(id);

            if (productToUpdate == null) {
                return new ErrorOr<Product>("Product not found");
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
                return new ErrorOr<Product>("Error while updating Product");
            }

            Product? updatedProduct = await _dal.GetProduct(id);

            if (updatedProduct == null) {
                return new ErrorOr<Product>("Error while getting Product");
            }

            return new ErrorOr<Product>(updatedProduct);
        }

        public async Task<ErrorOr<bool>> DeleteProduct(int id) {
            Product? productToDelete = await _dal.GetProduct(id);

            if (productToDelete == null) {
                return new ErrorOr<bool>("Product not found");
            }

            await _dal.DeleteProduct(productToDelete);
            return new ErrorOr<bool>(true);
        }
    }
}
