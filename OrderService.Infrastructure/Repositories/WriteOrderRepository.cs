using System;
using System.Threading.Tasks;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories;

public class WriteOrderRepository : IWriteOrderRepository
{
    private readonly WriteDbContext _db;

    public WriteOrderRepository(WriteDbContext db)
    {
        _db = db;
    }

    public async Task<Order> AddAsync(Order order)
    {
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }
}