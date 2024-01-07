using DAL.Repository.Models;

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
    }
}
