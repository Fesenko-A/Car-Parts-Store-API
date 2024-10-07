using DAL.Repository.Models;
using DAL.Services.Concrete.EF;

namespace BL {
    public class ShoppingCartBL {
        private readonly ShoppingCartDAL _shoppingCartDal;
        private readonly ProductDAL _productDal;
        private readonly CartItemDAL _cartItemDal;

        public ShoppingCartBL() {
            _shoppingCartDal = new ShoppingCartDAL();
            _productDal = new ProductDAL();
            _cartItemDal = new CartItemDAL();
        }

        public async Task<ErrorOr<ShoppingCart>> Get(string userId) {
            ShoppingCart? shoppingCart;

            if (string.IsNullOrEmpty(userId)) {
                shoppingCart = new ShoppingCart();
            }
            else {
                shoppingCart = await _shoppingCartDal.Get(userId);
            }

            if (shoppingCart == null) {
                return new ErrorOr<ShoppingCart>("Shopping Cart not found");
            }

            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Product.Price);

            return new ErrorOr<ShoppingCart>(shoppingCart);
        }

        public async Task<ErrorOr<bool>> Upsert(string userId, int productId, int updateQuantityBy) {
            ShoppingCart? shoppingCart = await _shoppingCartDal.Get(userId);
            Product? product = await _productDal.GetProduct(productId);

            if (product == null) {
                return new ErrorOr<bool>(false, "Product not found");
            }

            if (product.InStock == false) {
                return new ErrorOr<bool>(false, "Product is out of stock");
            }

            if (shoppingCart == null && updateQuantityBy > 0) {
                ShoppingCart newCart = new ShoppingCart { UserId = userId };
                await _shoppingCartDal.Create(newCart);

                CartItem newCartItem = new CartItem {
                    ProductId = productId,
                    Quantity = updateQuantityBy,
                    ShoppingCartId = newCart.Id,
                    Product = null
                };

                await _cartItemDal.Create(newCartItem);
            }
            else {
                CartItem? itemInCart = shoppingCart.CartItems.FirstOrDefault(x => x.ProductId == productId);

                if (itemInCart == null) {
                    if (updateQuantityBy <= 0) {
                        return new ErrorOr<bool>(false, "Cannot update updateQuantityBy with this value");
                    }

                    CartItem newCartItem = new CartItem {
                        ProductId = productId,
                        Quantity = updateQuantityBy,
                        ShoppingCartId = shoppingCart.Id,
                        Product = null
                    };

                    await _cartItemDal.Create(newCartItem);
                }
                else {
                    int newQuantity = itemInCart.Quantity + updateQuantityBy;
                    if (updateQuantityBy == 0 || newQuantity <= 0) {
                        await _cartItemDal.Remove(itemInCart);

                        if (shoppingCart.CartItems.Count() == 1) {
                            await _shoppingCartDal.Remove(shoppingCart);
                        }
                    }
                    else {
                        itemInCart.Quantity = newQuantity;
                        await _cartItemDal.Update(itemInCart);
                    }
                }
            }

            return new ErrorOr<bool>(true);
        }
    }
}
