using BL.Models;
using Common.Filters;
using DAL.Repository.Models;

namespace BL.Services.Interfaces {
    public interface IOrderBL {
        Task<(ErrorOr<List<Order>>, int)> GetAll(OrderFilters filters);

        Task<ErrorOr<Order>> Get(int id);

        Task<ErrorOr<Order>> Create(OrderCreateDto orderToCreate);

        Task<ErrorOr<Order>> Update(int id, OrderUpdateDto orderToUpdate);

    }
}
