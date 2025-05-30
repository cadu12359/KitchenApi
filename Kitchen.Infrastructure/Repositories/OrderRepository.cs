using Kitchen.Domain.Entities;
using Kitchen.Domain.Interfaces;
using Kitchen.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Kitchen.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddOrderAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }
    
    public async Task<Order> GetLastOrderAsync()
    {
        return await _context.Orders
            .OrderByDescending(o => o.OrderNumber)
            .FirstOrDefaultAsync();
    }
    
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Items) 
            .ToListAsync();
    }
}