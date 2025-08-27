using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IOrderEventRepository _eventRepo;

    public OrderService(IOrderRepository orderRepo, IOrderEventRepository eventRepo)
    {
        _orderRepo = orderRepo;
        _eventRepo = eventRepo;
    }

    public async Task<List<Order>> GetOrders(Guid tenantId)
    {
        return await _orderRepo.GetAllAsync(tenantId);
    }

    public async Task<Order?> GetOrder(Guid id, Guid tenantId)
    {
        return await _orderRepo.GetByIdAsync(id, tenantId);
    }

    public async Task CreateOrder(Order order)
    {
        await _orderRepo.AddAsync(order);
        var evt = new OrderEvent
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            TenantId = order.TenantId,
            EventType = "Created",
            Data = "Order created",
            OccurredAt = DateTime.UtcNow
        };
        await _eventRepo.AddEventAsync(evt);
    }
}