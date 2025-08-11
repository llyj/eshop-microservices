using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class OrderItemEntityTypeConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property<long>("OrderId");

        builder.Property(o => o.UnitPrice)
            .HasColumnType("decimal(32,4)");
    }
}