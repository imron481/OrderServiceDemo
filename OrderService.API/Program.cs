using System;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Infrastructure.Middleware;
using OrderService.Infrastructure.Services;
using OrderService.Infrastructure.Data;
using OrderService.Domain.Repositories;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Repositories;
using AppOrderService = OrderService.Application.Services.OrderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

// Configure dual database contexts
builder.Services.AddDbContext<WriteDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("WriteConnection"));
});

builder.Services.AddDbContext<ReadDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ReadConnection"));
});

// Configure legacy OrderDbContext with write connection for backward compatibility
builder.Services.AddDbContext<OrderDbContext>((serviceProvider, options) =>
{
    var tenantProvider = serviceProvider.GetRequiredService<ITenantProvider>();
    var schema = tenantProvider.GetSchema();
    
    options.UseSqlServer(builder.Configuration.GetConnectionString("WriteConnection"));
    // The schema will be handled in the context itself
});

// Register MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OrderService.Application.Queries.GetOrdersQuery).Assembly));

// Register repositories
builder.Services.AddScoped<IReadOrderRepository, ReadOrderRepository>();
builder.Services.AddScoped<IWriteOrderRepository, WriteOrderRepository>();
builder.Services.AddScoped<IWriteOrderEventRepository, WriteOrderEventRepository>();

// Register services
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddScoped<AppOrderService>();

// Keep legacy interfaces for backward compatibility
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderEventRepository, OrderEventRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<TenantMiddleware>();
app.MapControllers();

// Minimal API endpoints
app.MapGet("/orders", async (ITenantProvider tenantProvider, IMediator mediator) =>
{
    var tenantId = tenantProvider.GetTenantId();
    var query = new OrderService.Application.Queries.GetOrdersQuery(tenantId);
    var orders = await mediator.Send(query);
    return Results.Ok(orders);
});

app.MapPost("/orders", async (Order order, ITenantProvider tenantProvider, IMediator mediator) =>
{
    order.TenantId = tenantProvider.GetTenantId();
    order.Id = Guid.NewGuid();
    order.CreatedAt = DateTime.UtcNow;
    
    var command = new OrderService.Application.Commands.CreateOrderCommand(order);
    var createdOrder = await mediator.Send(command);
    return Results.Created($"/orders/{createdOrder.Id}", createdOrder);
});

app.UseMiddleware<TenantMiddleware>();
app.MapControllers();

app.Run();