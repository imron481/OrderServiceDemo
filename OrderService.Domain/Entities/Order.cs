namespace OrderService.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}