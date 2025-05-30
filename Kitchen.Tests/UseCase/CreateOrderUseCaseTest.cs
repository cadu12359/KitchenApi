using Kitchen.Application.DTOs;
using Kitchen.Application.UseCases.Order;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Interfaces;
using Kitchen.Infrastructure.Interfaces;
using Moq;

namespace Kitchen.Tests.UseCase;

public class CreateOrderUseCaseTest
{
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly Mock<IRabbitMQService> _mockRabbitMqService;
    private readonly CreateOrderUseCase _useCase;

    public CreateOrderUseCaseTest()
    {
        _mockOrderRepository = new Mock<IOrderRepository>();
        _mockRabbitMqService = new Mock<IRabbitMQService>();
        _useCase = new CreateOrderUseCase(_mockOrderRepository.Object, _mockRabbitMqService.Object);
    }

    [Fact]
    public async Task Execute_ShouldCreateOrderAndPublishMessages_WhenRequestIsValid()
    {
        var lastOrder = new Order { OrderNumber = 5 };
        var request = new CreateOrderRequestDto
        {
            Items = new List<OrderItemDto>
            {
                new OrderItemDto { ItemName = "Pizza", KitchenArea = KitchenArea.Fritos },
                new OrderItemDto { ItemName = "Coke", KitchenArea = KitchenArea.Bebidas }
            }
        };

        _mockOrderRepository
            .Setup(repo => repo.GetLastOrderAsync())
            .ReturnsAsync(lastOrder);

        _mockOrderRepository
            .Setup(repo => repo.AddOrderAsync(It.IsAny<Order>()))
            .Returns(Task.CompletedTask);

        var result = await _useCase.Execute(request);

        Assert.NotNull(result);
        Assert.Equal(6, result.OrderNumber); // O próximo número deve ser 6
        Assert.Equal(2, result.Items.Count);

        // Verifica se as mensagens foram publicadas corretamente
        _mockRabbitMqService.Verify(
            service => service.PublishMessage("fritos_queue", It.IsAny<OrderItem>()),
            Times.Once);
        _mockRabbitMqService.Verify(
            service => service.PublishMessage("bebidas_queue", It.IsAny<OrderItem>()),
            Times.Once);

        // Verifica se o pedido foi adicionado ao repositório
        _mockOrderRepository.Verify(repo => repo.AddOrderAsync(It.IsAny<Order>()), Times.Once);
    }


    [Fact]
    public async Task Execute_ShouldThrowException_WhenRepositoryThrowsException()
    {
        var request = new CreateOrderRequestDto
        {
            Items = new List<OrderItemDto>
            {
                new OrderItemDto { ItemName = "Salad", KitchenArea = KitchenArea.Saladas }
            }
        };

        _mockOrderRepository
            .Setup(repo => repo.GetLastOrderAsync())
            .ThrowsAsync(new Exception("Erro no repositório"));

        await Assert.ThrowsAsync<Exception>(() => _useCase.Execute(request));
    }
}