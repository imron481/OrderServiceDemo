using System;
using System.Collections.Generic;
using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Application.Queries;

public record GetOrdersQuery(Guid TenantId) : IRequest<List<Order>>;