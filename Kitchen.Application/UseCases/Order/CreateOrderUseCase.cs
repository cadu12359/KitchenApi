using Kitchen.Application.DTOs;
using Kitchen.Application.Interfaces;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Interfaces;
using Kitchen.Infrastructure.Interfaces;

namespace Kitchen.Application.UseCases.Order;

public class CreateOrderUseCase : ICreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IRabbitMQService _rabbitMQService;

    public CreateOrderUseCase(IOrderRepository orderRepository, IRabbitMQService rabbitMQService)
    {
        _orderRepository = orderRepository;
        _rabbitMQService = rabbitMQService;
    }

    public async Task<OrderDto> Execute(CreateOrderRequestDto orderDto)
    {
        var lastOrder = await _orderRepository.GetLastOrderAsync();
        var nextOrderNumber = lastOrder?.OrderNumber + 1 ?? 1;
        
        var order = new Domain.Entities.Order
        {
            OrderNumber = nextOrderNumber, 
            Items = orderDto.Items.Select(item => new OrderItem
            {
                ItemName = item.ItemName,
                KitchenArea = item.KitchenArea
            }).ToList()
        };

        await _orderRepository.AddOrderAsync(order);

        foreach (var item in order.Items)
        {
            var queueName = GetQueueNameByKitchenArea(item.KitchenArea);
            _rabbitMQService.PublishMessage(queueName, item);
        }

        var responseorderDto = new OrderDto 
        {
            OrderNumber = nextOrderNumber, 
            Items = orderDto.Items.Select(item => new OrderItemDto()
            {
                ItemName = item.ItemName,
                KitchenArea = item.KitchenArea
            }).ToList()
        };

        return responseorderDto;
    }

    private string GetQueueNameByKitchenArea(KitchenArea kitchenArea)
    {
        return kitchenArea switch
        {
            KitchenArea.Fritos => "fritos_queue",
            KitchenArea.Grelhados => "grelhados_queue",
            KitchenArea.Saladas => "saladas_queue",
            KitchenArea.Bebidas => "bebidas_queue",
            KitchenArea.Sobremesa => "sobremesa_queue",
            _ => throw new ArgumentException("Área da cozinha inválida")
        };
    }
}