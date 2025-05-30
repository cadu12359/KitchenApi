using Kitchen.Application.DTOs;

namespace Kitchen.Application.Interfaces;

public interface IGetOrderUseCase
{
    Task<List<OrderDto>> Execute();
}