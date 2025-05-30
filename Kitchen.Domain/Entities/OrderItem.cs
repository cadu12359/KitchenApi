using System.Text.Json.Serialization;

namespace Kitchen.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public string ItemName { get; set; }
    public KitchenArea KitchenArea { get; set; }
    public Guid OrderId { get; set; }
    [JsonIgnore]
    public Order Order { get; set; }
}