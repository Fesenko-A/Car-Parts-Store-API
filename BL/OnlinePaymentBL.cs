using Auth;
using BL.Models;
using DAL.Constants;
using DAL.Repository.Models;
using Stripe;

namespace BL {
    public class OnlinePaymentBL {
        private readonly DAL.OrderDAL _orderDAL;
        private readonly DAL.OnlinePaymentDAL _onlinePaymentDAL;
        private readonly DAL.ShoppingCartDAL _shoppingCartDAL;

        public OnlinePaymentBL() {
            _orderDAL = new DAL.OrderDAL();
            _onlinePaymentDAL = new DAL.OnlinePaymentDAL();
            _shoppingCartDAL = new DAL.ShoppingCartDAL();
        }

        public async Task<List<OnlinePayment>> GetAll(string? userId, string? status) {
            var payments = await _onlinePaymentDAL.GetAll(userId, status);
            return payments;
        }

        public async Task<ErrorOr<OnlinePayment?>> GetByOrderId(int orderId) {
            var payment = await _onlinePaymentDAL.GetByOrderId(orderId);
            
            if (payment == null) {
                return new ErrorOr<OnlinePayment?>("Online payment not found");
            }

            return new ErrorOr<OnlinePayment?>(payment);
        }

        public async Task<ErrorOr<OnlinePayment>> Create(int orderId, string paymentId) {
            Order? order = await _orderDAL.Get(orderId);

            if (order == null) {
                return new ErrorOr<OnlinePayment>("Order not found");
            }

            if (order.TotalItems == 0) {
                return new ErrorOr<OnlinePayment>("Order cannot have 0 totalItems");
            }

            OnlinePayment? paymentCheck = await _onlinePaymentDAL.GetByOrderId(orderId);
            if (paymentCheck != null) {
                return new ErrorOr<OnlinePayment>("Online payment already exists for this order");
            }

            OnlinePayment onlinePayment = new OnlinePayment {
                OrderId = orderId,
                PaymentId = paymentId,
                PaymentStatus = PaymentStatus.PAID,
                PaymentDate = DateTime.UtcNow,
                PaymentAmount = (double)order.OrderTotal,
                UserId = order.UserId
            };

            await _onlinePaymentDAL.Create(onlinePayment);

            OnlinePayment? paymentFromDb = await _onlinePaymentDAL.Get(onlinePayment.Id);
            if (paymentFromDb == null) {
                return new ErrorOr<OnlinePayment>("Error while creating online payment");
            }

            order.Paid = true;
            order.PaymentMethodId = 2;
            await _orderDAL.Update(order);

            return new ErrorOr<OnlinePayment>(onlinePayment);
        }

        public async Task<ErrorOr<OnlinePayment>> Cancel(int orderId) {
            Order? order = await _orderDAL.Get(orderId);

            if (order == null) {
                return new ErrorOr<OnlinePayment>("Order not found");
            }

            if (order.TotalItems == 0) {
                return new ErrorOr<OnlinePayment>("Order cannot have 0 totalItems");
            }

            if (order.PaymentMethodId != 2 && order.Paid != true) {
                return new ErrorOr<OnlinePayment>("Payment method for this order is set to cash");
            }

            OnlinePayment? onlinePayment = await _onlinePaymentDAL.GetByOrderId(orderId);
            if (onlinePayment == null) {
                return new ErrorOr<OnlinePayment>("No payment to refund (order not paid)");
            }

            StripeConfiguration.ApiKey = AuthOptions.STRIPEKEY;
            RefundCreateOptions options = new RefundCreateOptions { PaymentIntent = onlinePayment.PaymentId};
            RefundService service = new RefundService();

            try {
                var response = service.Create(options);
            } catch {
                return new ErrorOr<OnlinePayment>("Error while creating refund");
            }

            onlinePayment.PaymentStatus = PaymentStatus.RETURNED;

            bool updated = await Update(onlinePayment.Id, onlinePayment);
            if (updated == false) {
                return new ErrorOr<OnlinePayment>("Error while updating payment");
            }

            OnlinePayment? refundedPayment = await _onlinePaymentDAL.Get(onlinePayment.Id);
            if (refundedPayment == null) {
                return new ErrorOr<OnlinePayment>("Error while getting payment");
            }

            return new ErrorOr<OnlinePayment>(refundedPayment);
        }

        public async Task<ErrorOr<StripeIntent>> CreateIntent(string userId) {
            ShoppingCart? shoppingCart = await _shoppingCartDAL.Get(userId);

            if (shoppingCart == null) {
                return new ErrorOr<StripeIntent>("Shopping Cart not found");
            }

            if (shoppingCart.CartItems == null || shoppingCart.CartItems.Count() == 0) {
                return new ErrorOr<StripeIntent>("Shopping Cart is empty");
            }

            StripeConfiguration.ApiKey = AuthOptions.STRIPEKEY;
            shoppingCart.CartTotal = shoppingCart.CartItems.Sum(u => u.Quantity * u.Product.Price);

            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions {
                Amount = (int)(shoppingCart.CartTotal * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions {
                    Enabled = true,
                },
            };
            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = await service.CreateAsync(options);

            StripeIntent successfulIntent = new StripeIntent { PaymentId = response.Id, ClientSecret = response.ClientSecret };
            
            return new ErrorOr<StripeIntent>(successfulIntent);
        }

        public async Task<ErrorOr<StripeIntent>> CreateIntent(int orderId) {
            Order? order = await _orderDAL.Get(orderId);

            if (order == null) {
                return new ErrorOr<StripeIntent>("Order not found");
            }

            if (order.TotalItems == 0) {
                return new ErrorOr<StripeIntent>("Order is empty");
            }

            StripeConfiguration.ApiKey = AuthOptions.STRIPEKEY;

            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions {
                Amount = (int)(order.OrderTotal * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions {
                    Enabled = true,
                },
            };
            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = await service.CreateAsync(options);

            StripeIntent successfulIntent = new StripeIntent { PaymentId = response.Id, ClientSecret = response.ClientSecret };

            return new ErrorOr<StripeIntent>(successfulIntent);
        }

        private async Task<bool> Update(int id, OnlinePayment paymentToUpdate) {
            if (id != paymentToUpdate.Id) { 
                return false; 
            }

            OnlinePayment? paymentFromDb = await _onlinePaymentDAL.Get(id);

            if (paymentFromDb == null) { 
                return false;
            }

            paymentFromDb.LastUpdate = DateTime.UtcNow;
            paymentFromDb.PaymentStatus = paymentToUpdate.PaymentStatus;

            await _onlinePaymentDAL.Update(paymentFromDb);
            return true;
        }
    }
}
