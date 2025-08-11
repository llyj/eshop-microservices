using Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.EntityConfigurations;

public class SysUserEntityTypeConfiguration : IEntityTypeConfiguration<SysUser>
{
    public void Configure(EntityTypeBuilder<SysUser> builder)
    {
        builder.ToTable(nameof(SysUser));

        builder.HasKey(o => o.Id);

        builder.Property(o => o.UserName).HasMaxLength(30).IsRequired();

        builder.Property(o => o.Password).HasMaxLength(1000).IsRequired();

        builder.Property(o => o.PhoneNumber).HasMaxLength(200).IsRequired(false);
        builder.Property(o => o.Email).HasMaxLength(300).IsRequired();

        builder.Property(o => o.Remark).HasMaxLength(500);

        builder.HasIndex(o => o.UserName).IsUnique();

        builder.HasMany(o => o.Roles)
            .WithMany(o => o.SysUsers)
            .UsingEntity<SysUserRole>(nameof(SysUserRole),
                o => o.HasOne(s => s.Role).WithMany().HasForeignKey(s => s.RoleId),
                l => l.HasOne(s => s.User).WithMany().HasForeignKey(s => s.UserId)
                );
    }
}