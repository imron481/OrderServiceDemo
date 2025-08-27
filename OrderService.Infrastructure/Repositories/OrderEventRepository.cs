using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories;

public class OrderEventRepository : IOrderEventRepository
{
    private readonly OrderDbContext _db;

    public OrderEventRepository(OrderDbContext db)
    {
        _db = db;
    }

    public async Task<List<OrderEvent>> GetEventsAsync(Guid orderId, Guid tenantId)
    {
        return await _db.OrderEvents
            .Where(e => e.OrderId == orderId && e.TenantId == tenantId)
            .ToListAsync();
    }

    public async Task AddEventAsync(OrderEvent orderEvent)
    {
        _db.OrderEvents.Add(orderEvent);
        await _db.SaveChangesAsync();
    }
}