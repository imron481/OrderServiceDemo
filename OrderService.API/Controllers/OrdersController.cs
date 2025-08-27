using Microsoft.AspNetCore.Mvc;
using OrderService.Application.Services;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Services;

namespace OrderService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;
    private readonly ITenantProvider _tenantProvider;

    public OrdersController(OrderService orderService, ITenantProvider tenantProvider)
    {
        _orderService = orderService;
        _tenantProvider = tenantProvider;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var tenantId = _tenantProvider.GetTenantId();
        var orders = await _orderService.GetOrders(tenantId);
        return Ok(orders);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var order = await _orderService.GetOrder(id, tenantId);
        if (order == null) return NotFound();
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Order order)
    {
        order.TenantId = _tenantProvider.GetTenantId();
        await _orderService.CreateOrder(order);
        return CreatedAtAction(nameof(Get), new { id = order.Id }, order);
    }
}