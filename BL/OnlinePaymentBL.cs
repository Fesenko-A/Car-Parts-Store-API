using Auth;
using BL.Models;
using DAL.Constants;
using DAL.Repository.Models;
using Stripe;

namespace BL {
    public class OnlinePaymentBL {
        private readonly DAL.OrderDAL _orderDAL;
        private readonly DAL.OnlinePaymentDAL _onlinePaymentDAL;

        public OnlinePaymentBL() {
            _orderDAL = new DAL.OrderDAL();
            _onlinePaymentDAL = new DAL.OnlinePaymentDAL();
        }

        public async Task<List<OnlinePayment>> GetAll() {
            var payments = await _onlinePaymentDAL.GetAll();
            return payments;
        }

        public async Task<OnlinePayment?> GetByOrderId(int orderId) {
            OnlinePayment? payment = await _onlinePaymentDAL.GetByOrderId(orderId);
            return payment;
        }

        public async Task<OnlinePayment?> Create(int orderId) {
            Order? order = await _orderDAL.Get(orderId);

            if (order == null || order.TotalItems == 0) {
                return null;
            }

            OnlinePayment? paymentCheck = await _onlinePaymentDAL.GetByOrderId(orderId);
            if (paymentCheck != null) {
                return null;
            }

            StripeConfiguration.ApiKey = AuthOptions.STRIPEKEY;
            PaymentIntentCreateOptions options = new PaymentIntentCreateOptions {
                Amount = (int)(order.OrderTotal * 100),
                Currency = "usd",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions {
                    Enabled = true
                }
            };

            PaymentIntentService service = new PaymentIntentService();
            PaymentIntent response = service.Create(options);

            OnlinePayment onlinePayment = new OnlinePayment {
                OrderId = orderId,
                PaymentId = response.Id,
                PaymentStatus = PaymentStatus.PAID,
                PaymentDate = DateTime.UtcNow,
                ClientSecret = response.ClientSecret,
                PaymentAmount = (double)options.Amount / 100,
            };

            await _onlinePaymentDAL.Create(onlinePayment);

            OnlinePayment? paymentFromDb = await _onlinePaymentDAL.Get(onlinePayment.Id);
            if (paymentFromDb == null) {
                return null;
            }

            return onlinePayment;
        }

        public async Task<OnlinePayment?> Cancel(int orderId) {
            Order? order = await _orderDAL.Get(orderId);

            if (order == null || order.TotalItems == 0) {
                return null;
            }

            OnlinePayment? onlinePayment = await _onlinePaymentDAL.GetByOrderId(orderId);
            if (onlinePayment == null) {
                return null;
            }

            StripeConfiguration.ApiKey = AuthOptions.STRIPEKEY;
            RefundCreateOptions options = new RefundCreateOptions { PaymentIntent = onlinePayment.PaymentId};
            RefundService service = new RefundService();

            var response = service.Create(options);

            onlinePayment.PaymentStatus = PaymentStatus.RETURNED;
            await _onlinePaymentDAL.Update(onlinePayment);

            OnlinePayment? refundedPayment = await _onlinePaymentDAL.Get(onlinePayment.Id);
            return refundedPayment;
        }

        public async Task<bool> Update(int id, OnlinePaymentDto paymentDto) {
            if (id != paymentDto.Id) { 
                return false; 
            }

            OnlinePayment? paymentFromDb = await _onlinePaymentDAL.Get(id);

            if (paymentFromDb == null) { 
                return false;
            }

            paymentFromDb.LastUpdate = DateTime.UtcNow;
            paymentFromDb.PaymentStatus = paymentDto.PaymentStatus;

            await _onlinePaymentDAL.Update(paymentFromDb);
            return true;
        }
    }
}
