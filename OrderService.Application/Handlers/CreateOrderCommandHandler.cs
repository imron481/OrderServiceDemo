using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Commands;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Order>
{
    private readonly IWriteOrderRepository _writeRepository;
    private readonly IWriteOrderEventRepository _eventRepository;

    public CreateOrderCommandHandler(IWriteOrderRepository writeRepository, IWriteOrderEventRepository eventRepository)
    {
        _writeRepository = writeRepository;
        _eventRepository = eventRepository;
    }

    public async Task<Order> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _writeRepository.AddAsync(request.Order);
        
        var orderEvent = new OrderEvent
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            TenantId = order.TenantId,
            EventType = "Created",
            Data = "Order created",
            OccurredAt = DateTime.UtcNow
        };
        
        await _eventRepository.AddEventAsync(orderEvent);
        
        return order;
    }
}