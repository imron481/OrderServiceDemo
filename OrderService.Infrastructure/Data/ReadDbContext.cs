using System;
using Microsoft.EntityFrameworkCore;
using OrderService.Domain.Entities;

namespace OrderService.Infrastructure.Data;

public class ReadDbContext : DbContext
{
    private readonly string _schema;

    public ReadDbContext(DbContextOptions<ReadDbContext> options, string schema = "dbo")
        : base(options)
    {
        _schema = schema;
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderEvent> OrderEvents => Set<OrderEvent>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(_schema);

        modelBuilder.Entity<Order>().ToTable("Orders", _schema);
        modelBuilder.Entity<OrderItem>().ToTable("OrderItems", _schema);
        modelBuilder.Entity<OrderEvent>().ToTable("OrderEvents", _schema);
    }
}