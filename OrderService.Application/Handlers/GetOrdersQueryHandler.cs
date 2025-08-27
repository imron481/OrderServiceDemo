using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.Queries;
using OrderService.Domain.Entities;
using OrderService.Domain.Repositories;

namespace OrderService.Application.Handlers;

public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, List<Order>>
{
    private readonly IReadOrderRepository _readRepository;

    public GetOrdersQueryHandler(IReadOrderRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task<List<Order>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _readRepository.GetAllAsync(request.TenantId);
    }
}