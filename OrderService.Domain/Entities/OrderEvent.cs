namespace OrderService.Domain.Entities;

public class OrderEvent
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid TenantId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; }
}