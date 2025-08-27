using System;
using Microsoft.AspNetCore.Http;

namespace OrderService.Infrastructure.Services;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _ctx;

    public TenantProvider(IHttpContextAccessor ctx)
    {
        _ctx = ctx;
    }

    public Guid GetTenantId()
    {
        var val = _ctx.HttpContext?.Items["TenantId"];
        return val is Guid guid ? guid : Guid.Empty;
    }

    public string GetSchema()
    {
        var val = _ctx.HttpContext?.Items["TenantSchema"];
        return val?.ToString() ?? "dbo";
    }
}