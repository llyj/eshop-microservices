using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class SysMenuEntityTypeConfiguration : IEntityTypeConfiguration<SysMenu>
{
    public void Configure(EntityTypeBuilder<SysMenu> builder)
    {
        builder.ToTable(nameof(SysMenu));

        builder.Property(o => o.Description).HasMaxLength(500);
        builder.Property(o => o.LinkUrl).HasMaxLength(300);
        builder.Property(o => o.Name).HasMaxLength(100);

        builder.HasOne(o => o.Parent)
            .WithMany(o => o.Childrens)
            .HasForeignKey(o => o.ParentId);

        builder.HasMany(o => o.SysActions)
            .WithOne(o => o.SysMenu)
            .HasForeignKey(o => o.SysMenuId);
    }
}