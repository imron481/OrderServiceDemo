using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace OrderService.Infrastructure.Middleware;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var tenantIdStr = context.Request.Headers["X-Tenant-Id"].FirstOrDefault();
        if (Guid.TryParse(tenantIdStr, out var tenantId))
        {
            context.Items["TenantId"] = tenantId;
            context.Items["TenantSchema"] = tenantId == Guid.Parse("11111111-1111-1111-1111-111111111111") ? "tenant_a" : "tenant_b";
        }
        await _next(context);
    }
}