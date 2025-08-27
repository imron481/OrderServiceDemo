using System;
using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Application.Commands;

public record CreateOrderCommand(Order Order) : IRequest<Order>;