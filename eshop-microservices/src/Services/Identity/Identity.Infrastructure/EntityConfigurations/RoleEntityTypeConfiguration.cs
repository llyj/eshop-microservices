using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Name).HasMaxLength(200);

        builder.Property(o => o.Description).HasMaxLength(500);

        builder.Property(o => o.IsEnable).HasDefaultValue(false);

        // 配置自关联
        builder.HasOne(o => o.Parent).WithMany(o => o.Childrens).HasForeignKey(o => o.ParentId).IsRequired(false);

        builder.HasMany(o => o.SysMenus)
            .WithMany(o => o.Roles)
            .UsingEntity<RoleSysMenu>(nameof(RoleSysMenu),
                o => o.HasOne(r => r.SysMenu).WithMany().HasForeignKey(r => r.SysMenuId),
                o => o.HasOne(r => r.Role).WithMany().HasForeignKey(r => r.RoleId)
                );

        builder.HasMany(o => o.SysActions)
            .WithMany(o => o.Roles)
            .UsingEntity<RoleSysAction>(nameof(RoleSysAction),
                o => o.HasOne(r => r.SysAction).WithMany().HasForeignKey(r => r.SysActionId),
                o => o.HasOne(r => r.Role).WithMany().HasForeignKey(r => r.RoleId),
                o => o.HasOne(r => r.SysMenu).WithMany().HasForeignKey(r => r.SysMenuId)
                );

        //builder.HasMany(o => o.SysUsers).WithMany(o => o.Roles).UsingEntity(cfg => cfg.ToTable("UserRole"));
    }
}