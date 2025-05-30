using Kitchen.Domain.Entities;

namespace Kitchen.Domain.Interfaces;

public interface IOrderRepository
{
    Task AddOrderAsync(Order order);
    Task<Order?> GetLastOrderAsync();
    Task<List<Order>> GetAllOrdersAsync();
}