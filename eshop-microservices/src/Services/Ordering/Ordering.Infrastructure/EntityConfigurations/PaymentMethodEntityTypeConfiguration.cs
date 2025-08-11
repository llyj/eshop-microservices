using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModels.BuyerAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

public class PaymentMethodEntityTypeConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.ToTable(nameof(PaymentMethod));

        builder.HasKey(b => b.Id);

        builder.HasOne(b => b.CardType)
            .WithMany()
            .HasForeignKey("_cardTypeId");

        builder.Property("_cardTypeId")
            .HasColumnName("CardTypeId");

        builder.Property("_alias")
            .HasColumnName("Alias")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property("_cardNumber")
            .HasColumnName("CardNumber")
            .HasMaxLength(25)
            .IsRequired();

        builder.Property("_cardHolderName")
            .HasColumnName("CardHolderName")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property("_expiration")
            .HasColumnName("Expiration")
            .HasMaxLength(25);

        //TODO:怎么构建的外键？？
        builder.Property<long>("BuyerId");
    }
}