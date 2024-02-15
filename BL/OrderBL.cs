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

        public List<Order> GetAll(string? userId, string? searchString, string? status) {
            var ordersFromDb = _dal.GetAll(userId, searchString, status);
            return ordersFromDb;
        }

        public async Task<Order?> Get(int id) {
            Order? order = await _dal.Get(id);
            return order;
        }

        public async Task<Order?> Create(OrderCreateDto orderToCreate) {
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
                return null;
            }

            Order orderFromDb = await _dal.Get(order.OrderId);
            return orderFromDb;
        }

        public async Task<bool> Update(int id, OrderUpdateDto orderToUpdate) {
            if (orderToUpdate == null || id != orderToUpdate.OrderId) {
                return false;
            }

            Order orderFromDb = await _dal.Get(id);
            if (orderFromDb == null) {
                return false;
            }

            orderFromDb.LastUpdate = DateTime.UtcNow;
            orderFromDb.Paid = orderToUpdate.Paid;

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

            if (orderToUpdate.PaymentMethodId != null && orderToUpdate.PaymentMethodId != 0) {
                orderFromDb.PaymentMethodId = orderToUpdate.PaymentMethodId;
            }

            await _dal.Update(orderFromDb); 
            return true;
        }
    }
}
