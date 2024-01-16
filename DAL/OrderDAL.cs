using DAL.Repository;
using DAL.Repository.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL {
    public class OrderDAL {
        private readonly ApplicationDbContext _context;

        public OrderDAL() {
            _context = new ApplicationDbContext();
        }

        public List<Order> GetAll(string? userId, string searchString, string status) {
            IEnumerable<Order> orders = _context.Orders.
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Brand).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Category).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.SpecialTag).
                OrderByDescending(o => o.OrderId);

            if (!string.IsNullOrEmpty(userId)) {
                orders = orders.Where(u => u.UserId == userId);
            }

            if (!string.IsNullOrEmpty(searchString)) {
                orders = orders.Where(u =>
                    u.PickupPhoneNumber.ToLower().Contains(searchString.ToLower()) ||
                    u.PickupEmail.ToLower().Contains(searchString.ToLower()) ||
                    u.PickupName.ToLower().Contains(searchString.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(status)) {
                orders = orders.Where(u => u.Status.ToLower() == status.ToLower());
            }

            return orders.ToList();
        }

        public async Task<Order> Get(int id) {
            var orders = await _context.Orders.Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Brand).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Category).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.SpecialTag).
                Where(x => x.OrderId == id).
                ToListAsync();

            var order = orders.FirstOrDefault();
            return order;
        }

        public void Create(Order orderToCreate) {
            _context.Orders.Add(orderToCreate);
        }

        public void CreateDetails(OrderDetails orderDetails) {
            _context.OrderDetails.Add(orderDetails);
        }

        public async Task Update(Order orderToUpdate) {
            _context.Orders.Update(orderToUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task Save() {
            await _context.SaveChangesAsync();
        }
    }
}
