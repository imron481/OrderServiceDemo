# OrderServiceDemo

Demo microservice .NET untuk multi-tenant, sharding, dan event sourcing.

## Fitur
- Multi-tenant dengan schema per tenant
- Event sourcing: event tercatat di tabel terpisah
- Sharding manual via schema dan middleware
- API sederhana untuk CRUD order

## Cara Jalan
1. Jalankan migrasi database dengan schema "tenant_a", "tenant_b"
2. Jalankan API
3. Gunakan header `X-Tenant-Id` pada setiap request

## Struktur Kode
- Domain: Entity dan repository interface
- Infrastructure: Data, repository, middleware, tenant provider
- Application: Business logic
- API: Controller dan startup

## Contoh Request
```
GET /api/orders
X-Tenant-Id: [tenant guid]
```