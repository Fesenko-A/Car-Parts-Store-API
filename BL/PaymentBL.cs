using Auth;
using DAL.Repository.Models;
using Stripe;

namespace BL {
    public class PaymentBL {
        private readonly DAL.ShoppingCartDAL _shoppingCartDAL;

        public PaymentBL() {
            _shoppingCartDAL = new DAL.ShoppingCartDAL();
        }

        public async Task<ShoppingCart?> MakePayment(string userId) {
            ShoppingCart? shoppingCart = await _shoppingCartDAL.Get(userId);

            if (shoppingCart == null || shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0) {
                return null;
            }

            StripeConfiguration.ApiKey = AuthOptions.STRIPEKEY;
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(i => i.Quantity * i.Product.Price);

            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions {
                Amount = (int)(shoppingCart.CartTotal * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions {
                    Enabled = true
                }
            };

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = service.Create(options);
            shoppingCart.PaymentId = response.Id;
            shoppingCart.ClientSecret = response.ClientSecret;

            return shoppingCart;
        }
    }
}
