using OrderService.Infrastructure.Middleware;
using OrderService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITenantProvider, TenantProvider>();
builder.Services.AddHttpContextAccessor();
// Add your DbContext, repositories, and services registrations here.

var app = builder.Build();

app.UseMiddleware<TenantMiddleware>();
app.MapControllers();

app.Run();