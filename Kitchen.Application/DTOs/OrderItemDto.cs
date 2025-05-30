using System.ComponentModel.DataAnnotations;
using Kitchen.Domain.Entities;

namespace Kitchen.Application.DTOs;

public class OrderItemDto
{
    [Required(ErrorMessage = "O nome do item é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do item não pode ter mais de 100 caracteres.")]
    public string ItemName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "A área da cozinha é obrigatória.")]
    [Range(0, 4, ErrorMessage = "A área da cozinha deve ser um valor inteiro entre 0 e 4.")]
    public KitchenArea KitchenArea { get; set; }
}