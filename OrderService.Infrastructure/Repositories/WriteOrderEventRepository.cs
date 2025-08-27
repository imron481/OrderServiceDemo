using System;
using System.Threading.Tasks;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;
using OrderService.Infrastructure.Data;

namespace OrderService.Infrastructure.Repositories;

public class WriteOrderEventRepository : IWriteOrderEventRepository
{
    private readonly WriteDbContext _db;

    public WriteOrderEventRepository(WriteDbContext db)
    {
        _db = db;
    }

    public async Task AddEventAsync(OrderEvent orderEvent)
    {
        _db.OrderEvents.Add(orderEvent);
        await _db.SaveChangesAsync();
    }
}