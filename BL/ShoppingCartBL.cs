using DAL.Repository.Models;

namespace BL {
    public class ShoppingCartBL {
        private readonly DAL.ShoppingCartDAL _shoppingCartDal;
        private readonly DAL.ProductDAL _productDal;
        private readonly DAL.CartItemDAL _cartItemDal;
        public ShoppingCartBL() {
            _shoppingCartDal = new DAL.ShoppingCartDAL();
            _productDal = new DAL.ProductDAL();
            _cartItemDal = new DAL.CartItemDAL();
        }

        public async Task<ShoppingCart?> Get(string userId) {
            ShoppingCart? shoppingCart;

            if (string.IsNullOrEmpty(userId)) {
                shoppingCart = new ShoppingCart();
            }
            else {
                shoppingCart = await _shoppingCartDal.Get(userId);
            }

            if (shoppingCart == null) {
                return null;
            }

            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Product.Price);

            return shoppingCart;
        }

        public async Task<bool> Upsert(string userId, int productId, int updateQuantityBy) {
            ShoppingCart? shoppingCart = await _shoppingCartDal.Get(userId);
            Product? product = await _productDal.GetProduct(productId);

            if (product == null) { 
                return false; 
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
                        return false;
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
                        await _cartItemDal.Create(itemInCart);

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

            return true;
        }
    }
}
