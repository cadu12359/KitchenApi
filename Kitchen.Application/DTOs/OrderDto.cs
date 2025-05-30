using System.ComponentModel.DataAnnotations;

namespace Kitchen.Application.DTOs;

public class OrderDto
{
    public OrderDto(List<OrderItemDto> items)
    {
        Items = items;
    }

    public OrderDto()
    {
    }

    public int OrderNumber { get; set; }
    
    [Required(ErrorMessage = "A lista de itens é obrigatória.")]
    [MinLength(1, ErrorMessage = "A lista de itens deve conter pelo menos um item.")]
    public List<OrderItemDto>? Items { get; set; }
}