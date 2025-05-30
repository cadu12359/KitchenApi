namespace Kitchen.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public int OrderNumber { get; set; }
    public DateTime DataPedido { get; private set; } = DateTime.UtcNow;
    public List<OrderItem> Items { get; set; } = new List<OrderItem>();
}