using DotNetCore.CAP;
using Identity.Domain;
using Identity.Infrastructure.EntityConfigurations;
using MediatR;
using MicroServiceDemo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure;

public class IdentityDbContext : EFContext
{
    public IdentityDbContext(DbContextOptions options, IMediator mediator, ICapPublisher publisher) : base(options, mediator, publisher)
    {
    }

    public DbSet<SysUser> SysUsers { get; set; }

    public DbSet<Role> Roles { get; set; }
    public DbSet<SysMenu> SysMenus { get; set; }
    public DbSet<SysAction> SysActions { get; set; }
    //public DbSet<SysUserRole> SysUserRoles { get; set; }
    //public DbSet<RoleSysMenu> RoleSysMenus { get; set; }
    //public DbSet<RoleSysAction> RoleSysActions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new SysUserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SysMenuEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new SysActionEntityTypeConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}