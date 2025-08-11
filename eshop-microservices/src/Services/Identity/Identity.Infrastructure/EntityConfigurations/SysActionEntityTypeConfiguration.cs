using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class SysActionEntityTypeConfiguration : IEntityTypeConfiguration<SysAction>
{
    public void Configure(EntityTypeBuilder<SysAction> builder)
    {
        builder.ToTable(nameof(SysAction));

        builder.Property(o => o.Name).HasMaxLength(100);
        builder.Property(o => o.Action).HasMaxLength(100);
        builder.Property(o => o.Contronller).HasMaxLength(100);
        builder.Property(o => o.ApiUrl).HasMaxLength(200);
    }
}