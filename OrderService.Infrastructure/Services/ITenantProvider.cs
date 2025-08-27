using System;

namespace OrderService.Infrastructure.Services;

public interface ITenantProvider
{
    Guid GetTenantId();
    string GetSchema();
}