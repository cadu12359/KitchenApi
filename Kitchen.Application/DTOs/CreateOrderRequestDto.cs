using System.ComponentModel.DataAnnotations;

namespace Kitchen.Application.DTOs;

public class CreateOrderRequestDto
{
    public CreateOrderRequestDto(List<OrderItemDto> items)
    {
        Items = items;
    }

    public CreateOrderRequestDto()
    {
    }
    
    [Required(ErrorMessage = "A lista de itens é obrigatória.")]
    [MinLength(1, ErrorMessage = "A lista de itens deve conter pelo menos um item.")]
    public List<OrderItemDto>? Items { get; set; }
}