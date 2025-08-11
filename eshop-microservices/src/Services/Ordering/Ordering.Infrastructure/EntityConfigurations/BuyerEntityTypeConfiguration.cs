using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModels.BuyerAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class BuyerEntityTypeConfiguration : IEntityTypeConfiguration<Buyer>
{
    public void Configure(EntityTypeBuilder<Buyer> builder)
    {
        builder.ToTable(nameof(Buyer));

        builder.Property(b => b.IdentityGuid)
            .HasMaxLength(200)
            .IsRequired();

        //builder.Property(b => b.Name)
        //    .HasColumnType("text");

        //构建索引
        builder.HasIndex(b => b.IdentityGuid)
            .IsUnique(true);

        builder.HasMany(b => b.PaymentMethods)
            .WithOne();
    }
}