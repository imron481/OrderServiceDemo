using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IReadOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id, Guid tenantId);
    Task<List<Order>> GetAllAsync(Guid tenantId);
}