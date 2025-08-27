using System;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Domain.Repositories;

public interface IWriteOrderRepository
{
    Task<Order> AddAsync(Order order);
}