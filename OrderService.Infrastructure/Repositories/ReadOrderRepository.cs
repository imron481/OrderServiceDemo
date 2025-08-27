using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories;

public class ReadOrderRepository : IReadOrderRepository
{
    private readonly ReadDbContext _db;

    public ReadOrderRepository(ReadDbContext db)
    {
        _db = db;
    }

    public async Task<Order?> GetByIdAsync(Guid id, Guid tenantId)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id && o.TenantId == tenantId);
    }

    public async Task<List<Order>> GetAllAsync(Guid tenantId)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .Where(o => o.TenantId == tenantId)
            .ToListAsync();
    }
}