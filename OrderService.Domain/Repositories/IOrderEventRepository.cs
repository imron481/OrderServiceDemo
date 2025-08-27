using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IOrderEventRepository
{
    Task<List<OrderEvent>> GetEventsAsync(Guid orderId, Guid tenantId);
    Task AddEventAsync(OrderEvent orderEvent);
}