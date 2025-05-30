using Kitchen.Application.DTOs;
using Kitchen.Application.Interfaces;
using Kitchen.Domain.Interfaces;

namespace Kitchen.Application.UseCases.Order;

public class GetOrderUseCase : IGetOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<List<OrderDto>> Execute()
    {
        var orders = await _orderRepository.GetAllOrdersAsync();
        
        var orderDtos = orders.Select(o => new OrderDto
        {
            OrderNumber = o.OrderNumber,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ItemName = i.ItemName,
                KitchenArea = i.KitchenArea
            }).ToList()
        }).ToList();

        return orderDtos;
    }
    
}