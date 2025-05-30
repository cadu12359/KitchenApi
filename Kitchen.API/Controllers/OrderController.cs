using Kitchen.Application.DTOs;
using Kitchen.Application.Interfaces;
using Kitchen.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Kitchen.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly ICreateOrderUseCase _createOrderUseCase;
    private readonly IGetOrderUseCase _getOrderUseCase;

    public OrderController(ICreateOrderUseCase createOrderUseCase, IGetOrderUseCase getOrderUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _getOrderUseCase = getOrderUseCase;
    }

    /// <summary>
    /// Create an Order on the Kitchen
    /// </summary>
    /// <param name="orderDto"></param>
    /// <returns></returns>
    [HttpPost("CreateOrder")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto orderDto)
    {
        if (!ModelState.IsValid) 
        {
            return BadRequest(ModelState); 
        }
        
        var order = await _createOrderUseCase.Execute(orderDto);
        return Ok(order);
    }
    
    /// <summary>
    /// Get All Orders
    /// </summary>
    /// <param name="getOrderUseCase"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _getOrderUseCase.Execute();
        
        return Ok(orders);
    }
}