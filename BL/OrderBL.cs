using BL.Models;
using DAL.Constants;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace BL {
    public class OrderBL {
        private readonly DAL.OrderDAL _dal;

        public OrderBL() {
            _dal = new DAL.OrderDAL();
        }

        public async Task<List<Order>> GetAll(string? userId, string? searchString, string? status) {
            var ordersFromDb = await _dal.GetAll(userId, searchString, status);
            return ordersFromDb;
        }

        public async Task<ErrorOr<Order>> Get(int id) {
            Order? order = await _dal.Get(id);

            if (order == null) {
                return new ErrorOr<Order>("Order not found");
            }

            return new ErrorOr<Order>(order);
        }

        public async Task<ErrorOr<Order>> Create(OrderCreateDto orderToCreate) {
            Order order = new Order {
                UserId = orderToCreate.UserId,
                PickupEmail = orderToCreate.PickupEmail,
                PickupName = orderToCreate.PickupName,
                PickupPhoneNumber = orderToCreate.PickupPhoneNumber,
                OrderTotal = orderToCreate.OrderTotal,
                OrderDate = DateTime.UtcNow,
                PaymentMethodId = orderToCreate.PaymentMethodId,
                TotalItems = orderToCreate.TotalItems,
                Status = string.IsNullOrEmpty(orderToCreate.Status) ? OrderStatus.PENDING : orderToCreate.Status,
                Paid = false
            };

            try {
                await _dal.Create(order);

                foreach (var orderDetailsDto in orderToCreate.OrderDetails) {
                    OrderDetails orderDetails = new OrderDetails {
                        OrderId = order.OrderId,
                        ProductName = orderDetailsDto.ProductName,
                        ProductId = orderDetailsDto.ProductId,
                        Price = orderDetailsDto.Price,
                        Quantity = orderDetailsDto.Quantity
                    };

                    _dal.CreateDetails(orderDetails);
                }

                await _dal.Save();
            }
            catch (DbUpdateException) {
                return new ErrorOr<Order>("Error while creating Order");
            }

            Order orderFromDb = await _dal.Get(order.OrderId);

            if (orderFromDb == null) {
                return new ErrorOr<Order>("Error while getting Order");
            }

            return new ErrorOr<Order>(orderFromDb);
        }

        public async Task<ErrorOr<Order>> Update(int id, OrderUpdateDto orderToUpdate) {
            if (orderToUpdate == null) {
                return new ErrorOr<Order>("Update body empty");
            }

            if (id != orderToUpdate.OrderId) {
                return new ErrorOr<Order>("IDs not match");
            }

            Order orderFromDb = await _dal.Get(id);
            if (orderFromDb == null) {
                return new ErrorOr<Order>("Order not found");
            }

            orderFromDb.LastUpdate = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(orderToUpdate.PickupName)) {
                orderFromDb.PickupName = orderToUpdate.PickupName;
            }

            if (!string.IsNullOrEmpty(orderToUpdate.PickupPhoneNumber)) {
                orderFromDb.PickupPhoneNumber = orderToUpdate.PickupPhoneNumber;
            }

            if (!string.IsNullOrEmpty(orderToUpdate.Status)) {
                orderFromDb.Status = orderToUpdate.Status;
            }

            if (!string.IsNullOrEmpty(orderToUpdate.PickupEmail)) {
                orderFromDb.PickupEmail = orderToUpdate.PickupEmail;
            }

            if (orderToUpdate.Paid != null) {
                orderFromDb.Paid = orderToUpdate.Paid;
            }

            if (orderToUpdate.PaymentMethodId != null && orderToUpdate.PaymentMethodId != 0) {
                orderFromDb.PaymentMethodId = orderToUpdate.PaymentMethodId;
            }

            return new ErrorOr<Order>(orderFromDb);
        }
    }
}
