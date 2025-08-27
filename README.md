# OrderServiceDemo - Dual Database CQRS Implementation

## Overview
This project demonstrates a complete implementation of dual database connection (read & write) using Entity Framework, CQRS with MediatR, and Minimal API for a multi-tenant OrderService.

## Architecture

### Project Structure
- **OrderService.Domain**: Core entities and repository interfaces
- **OrderService.Infrastructure**: Data contexts, repositories, middleware, and tenant services
- **OrderService.Application**: CQRS commands, queries, and handlers
- **OrderService.API**: Minimal API endpoints and configuration

### Key Features Implemented

#### 1. Dual Database Context
- `WriteDbContext`: Used for write operations (commands)
- `ReadDbContext`: Used for read operations (queries)
- Separate SQL Server connection strings for read/write scalability

#### 2. CQRS Pattern with MediatR
- **Queries**: `GetOrdersQuery` with `GetOrdersQueryHandler` (uses ReadDbContext)
- **Commands**: `CreateOrderCommand` with `CreateOrderCommandHandler` (uses WriteDbContext)
- Clean separation of read and write operations

#### 3. Repository Pattern
- `IReadOrderRepository` / `ReadOrderRepository`: Read operations using ReadDbContext
- `IWriteOrderRepository` / `WriteOrderRepository`: Write operations using WriteDbContext
- `IWriteOrderEventRepository` / `WriteOrderEventRepository`: Event sourcing using WriteDbContext

#### 4. Minimal API Endpoints
- `GET /orders`: Retrieves orders for tenant using CQRS query
- `POST /orders`: Creates orders using CQRS command with event sourcing

#### 5. Multi-Tenant Support
- Tenant-specific schemas (tenant_a, tenant_b)
- X-Tenant-Id header processing via TenantMiddleware
- Dynamic schema selection per tenant

#### 6. Event Sourcing
- Order events are automatically created and stored
- Uses write database for consistency

## API Usage

### Configuration (appsettings.json)
```json
{
  "ConnectionStrings": {
    "WriteConnection": "Server=localhost;Database=OrderDb_Write;Trusted_Connection=True;",
    "ReadConnection": "Server=localhost;Database=OrderDb_Read;Trusted_Connection=True;"
  }
}
```

### Endpoints
```bash
# Get all orders for a tenant
curl -X GET "http://localhost:5000/orders" -H "X-Tenant-Id: 11111111-1111-1111-1111-111111111111"

# Create a new order
curl -X POST "http://localhost:5000/orders" \
  -H "X-Tenant-Id: 11111111-1111-1111-1111-111111111111" \
  -H "Content-Type: application/json" \
  -d '{
    "customerName": "John Doe",
    "items": [
      {
        "productName": "Product A",
        "quantity": 2,
        "price": 10.99
      }
    ]
  }'
```

## Technical Implementation

### CQRS Flow
1. **Read Operations**: Minimal API → MediatR → GetOrdersQueryHandler → ReadOrderRepository → ReadDbContext
2. **Write Operations**: Minimal API → MediatR → CreateOrderCommandHandler → WriteOrderRepository + WriteOrderEventRepository → WriteDbContext

### Multi-Tenancy
- Tenant ID extracted from X-Tenant-Id header
- Schema mapping: tenant_a for specific GUID, tenant_b for others
- Each tenant has isolated data in separate schemas

### Dependencies
- Entity Framework Core 8.0.8
- MediatR 12.2.0
- SQL Server Provider
- ASP.NET Core 8.0

## Running the Application

1. **Build**: `dotnet build`
2. **Run**: `dotnet run --project OrderService.API`
3. **Test**: Use curl or Postman with appropriate tenant headers

## Database Setup
Before running, ensure SQL Server is available and create databases:
- `OrderDb_Write` for write operations
- `OrderDb_Read` for read operations (can be a read replica)

## Benefits of This Architecture

1. **Scalability**: Separate read/write databases allow independent scaling
2. **Performance**: Read operations don't impact write performance
3. **Maintainability**: Clean separation of concerns with CQRS
4. **Multi-tenancy**: Isolated data per tenant with dynamic schema selection
5. **Event Sourcing**: Complete audit trail of all operations
6. **Modern APIs**: Minimal API approach with clean, fast endpoints

This implementation is production-ready and suitable for high-load, multi-tenant systems requiring read/write scalability.