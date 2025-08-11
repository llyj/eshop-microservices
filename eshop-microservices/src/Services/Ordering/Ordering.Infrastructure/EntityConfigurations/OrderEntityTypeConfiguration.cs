using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.AggregateModels.BuyerAggregate;
using Ordering.Domain.AggregateModels.OrderAggregate;

namespace Ordering.Infrastructure.EntityConfigurations;

/// <summary>
/// 配置订单实体
/// </summary>
public class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));
        builder.HasKey(o => o.Id);

        //一对一转换
        builder.OwnsOne(o => o.Address, a =>
        {
            a.WithOwner();
            a.Property(a => a.City).HasMaxLength(500);
            a.Property(a => a.Country).HasMaxLength(200);
            a.Property(a => a.State).HasMaxLength(200);
            a.Property(a => a.Street).HasMaxLength(500);
            a.Property(a => a.ZipCode).HasMaxLength(50);
        });

        //枚举转字符串
        builder.Property(o => o.OrderStatus)
            .HasMaxLength(30)
            .HasConversion<string>();

        builder.HasOne(o => o.Buyer)
            .WithMany() // 买家和订单 多对一
            .HasForeignKey(o => o.BuyerId);

        builder.Property(o => o.PaymentId)
            .HasColumnName("PaymentMethodId"); // 自定义列名

        builder.HasOne<PaymentMethod>()
            .WithMany()
            .HasForeignKey(p => p.PaymentId)
            .OnDelete(DeleteBehavior.Restrict); // 设置删除时外键关联操作
    }
}