using Common.Filters;
using DAL.Repository.Models;

namespace DAL.Services.Interfaces {
    internal interface IOrderDAL {
        Task<(List<Order>, int)> GetAll(OrderFilters filters);

        Task<Order> Get(int id);

        Task Create(Order orderToCreate);

        void CreateDetails(OrderDetails orderDetails);

        Task Update(Order orderToUpdate);

        Task Save();
    }
}
