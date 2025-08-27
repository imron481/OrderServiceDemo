using System;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IWriteOrderEventRepository
{
    Task AddEventAsync(OrderEvent orderEvent);
}