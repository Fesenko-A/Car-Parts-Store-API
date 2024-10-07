using Common.Filters;
using DAL.Repository;
using DAL.Repository.Models;
using DAL.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Services.Concrete.EF
{
    public class OrderDAL : IOrderDAL {
        private readonly ApplicationDbContext _context;

        public OrderDAL() {
            _context = new ApplicationDbContext();
        }

        public async Task<(List<Order>, int)> GetAll(OrderFilters filters) {
            IQueryable<Order> orders = _context.Orders.
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Brand).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Category).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.SpecialTag).
                Include(m => m.PaymentMethod).
                OrderByDescending(o => o.OrderId);

            if (!string.IsNullOrEmpty(filters.UserId)) {
                orders = orders.Where(u => u.UserId == filters.UserId);
            }

            if (!string.IsNullOrEmpty(filters.SearchString)) {
                orders = orders.Where(u =>
                    u.PickupPhoneNumber.ToLower().Contains(filters.SearchString.ToLower()) ||
                    u.PickupEmail.ToLower().Contains(filters.SearchString.ToLower()) ||
                    u.PickupName.ToLower().Contains(filters.SearchString.ToLower())
                );
            }

            if (!string.IsNullOrEmpty(filters.Status)) {
                orders = orders.Where(u => u.Status.ToLower() == filters.Status.ToLower());
            }

            int totalRecords = orders.Count();
            orders = orders.Skip((filters.PageNumber - 1) * filters.PageSize).Take(filters.PageSize);
            var result = await orders.ToListAsync();

            return (result, totalRecords);
        }

        public async Task<Order> Get(int id) {
            var orders = await _context.Orders.
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Brand).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.Category).
                Include(o => o.OrderDetails).
                    ThenInclude(i => i.Product).
                    ThenInclude(p => p.SpecialTag).
                Include(m => m.PaymentMethod).
                Where(x => x.OrderId == id).
                ToListAsync();

            var order = orders.FirstOrDefault();
            return order;
        }

        public async Task Create(Order orderToCreate) {
            _context.Orders.Add(orderToCreate);
            await Save();
        }

        public void CreateDetails(OrderDetails orderDetails) {
            _context.OrderDetails.Add(orderDetails);
        }

        public async Task Update(Order orderToUpdate) {
            _context.Orders.Update(orderToUpdate);
            await Save();
        }

        public async Task Save() {
            await _context.SaveChangesAsync();
        }
    }
}
