using Kitchen.Application.DTOs;

namespace Kitchen.Application.Interfaces;

public interface ICreateOrderUseCase
{
    Task<OrderDto> Execute(CreateOrderRequestDto orderDto);
}