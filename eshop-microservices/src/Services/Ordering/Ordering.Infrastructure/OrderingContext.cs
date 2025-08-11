using DotNetCore.CAP;
using MediatR;
using MicroServiceDemo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.AggregateModels.BuyerAggregate;
using Ordering.Domain.AggregateModels.OrderAggregate;
using Ordering.Infrastructure.EntityConfigurations;

namespace Ordering.Infrastructure;

public class OrderingContext : EFContext
{
    public OrderingContext(DbContextOptions options, IMediator mediator, ICapPublisher publisher) : base(options, mediator, publisher)
    {
    }

    public DbSet<Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    public DbSet<Buyer> Buyers { get; set; }

    public DbSet<PaymentMethod> PaymentMethods { get; set; }

    public DbSet<CardType> CardTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new OrderEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}