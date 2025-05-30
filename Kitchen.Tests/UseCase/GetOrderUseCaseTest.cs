using FluentAssertions;
using Kitchen.Application.UseCases.Order;
using Kitchen.Domain.Entities;
using Kitchen.Domain.Interfaces;
using Moq;

namespace Kitchen.Tests.UseCase;

public class GetOrderUseCaseTests
{
    private readonly Mock<IOrderRepository> _mockRepository;
    private readonly GetOrderUseCase _useCase;
    
    public GetOrderUseCaseTests()
    {
        _mockRepository = new Mock<IOrderRepository>();

        _useCase = new GetOrderUseCase(_mockRepository.Object);
    }
    
    [Fact]
    public async Task Execute_ShouldReturnListOfOrderDtos_WhenOrdersExist()
    {
        var orders = new List<Order>
        {
            new Order
            {
                OrderNumber = 1,
                Items = new List<OrderItem>
                {
                    new OrderItem { ItemName = "Pizza", KitchenArea = KitchenArea.Grelhados },
                }
            },
            new Order
            {
                OrderNumber = 2,
                Items = new List<OrderItem>
                {
                    new OrderItem { ItemName = "Burger", KitchenArea = KitchenArea.Fritos }
                }
            }
        };

        _mockRepository
            .Setup(repo => repo.GetAllOrdersAsync())
            .ReturnsAsync(orders);

        // Act
        var result = await _useCase.Execute();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        // Verifica o primeiro pedido
        var firstOrder = result[0];
        firstOrder.OrderNumber.Should().Be(1);
        firstOrder.Items.Should().HaveCount(1);
        firstOrder.Items[0].ItemName.Should().Be("Pizza");
        firstOrder.Items[0].KitchenArea.Should().Be(KitchenArea.Grelhados);

        // Verifica o segundo pedido
        var secondOrder = result[1];
        secondOrder.OrderNumber.Should().Be(2);
        secondOrder.Items.Should().ContainSingle();
        secondOrder.Items[0].ItemName.Should().Be("Burger");
        secondOrder.Items[0].KitchenArea.Should().Be(KitchenArea.Fritos);

        _mockRepository.Verify(repo => repo.GetAllOrdersAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnEmptyList_WhenNoOrdersExist()
    {
        _mockRepository
            .Setup(repo => repo.GetAllOrdersAsync())
            .ReturnsAsync(new List<Order>());

        // Act
        var result = await _useCase.Execute();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);

        _mockRepository.Verify(repo => repo.GetAllOrdersAsync(), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenRepositoryThrowsException()
    {
        _mockRepository
            .Setup(repo => repo.GetAllOrdersAsync())
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _useCase.Execute());
        Assert.Equal("Database error", exception.Message);

        _mockRepository.Verify(repo => repo.GetAllOrdersAsync(), Times.Once);
    }
}