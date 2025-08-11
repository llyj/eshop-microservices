using Identity.Domain;
using Identity.Infrastructure;
using MicroServiceDemo.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Infrastructure;

public class IdentityDbContextSeed : IDbSeeder<IdentityDbContext>
{
    public async Task SeedAsync(IdentityDbContext context)
    {
        //if (!await context.SysUsers.AnyAsync())
        //{
        //    await context.SysUsers.AddRangeAsync(SysUserDataSeed());
        //    await context.SaveChangesAsync();
        //}
        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(RoleDataSeed());
            await context.SaveChangesAsync();
        }
        //if (!await context.SysMenus.AnyAsync())
        //{
        //    await context.SysMenus.AddRangeAsync(SysMenuDataSeed());
        //    await context.SaveChangesAsync();
        //}
        //if (!await context.SysActions.AnyAsync())
        //{
        //    await context.SysActions.AddRangeAsync(SysActionDataSeed());
        //    await context.SaveChangesAsync();
        //}
        //await context.SysUserRoles.AddRangeAsync(SysUserRoleDataSeed());
        //await context.RoleSysMenus.AddRangeAsync(RoleSysMenuDataSeed());
        //await context.RoleSysActions.AddRangeAsync(RoleSysActionDataSeed());
    }

    public static IEnumerable<SysUser> SysUserDataSeed()
    {
        yield return new SysUser() { UserName = "admin", Password = "admin", Email = "123@123.com", Status = 1 };
        yield return new SysUser() { UserName = "1", Password = "123", Email = "2@2.com", Status = 1 };
        yield return new SysUser() { UserName = "2", Password = "123", Email = "2@2.com", Status = 1 };
        yield return new SysUser() { UserName = "3", Password = "123", Email = "3@3.com", Status = 1 };
        yield return new SysUser() { UserName = "4", Password = "123", Email = "4@4.com", Status = 1 };
    }

    public static IEnumerable<Role> RoleDataSeed()
    {
        yield return new Role()
        {
            Name = "SuperAdmin",
            IsEnable = true,
            SysUsers = [new SysUser() { UserName = "admin", Password = "admin", Email = "4@4.com", Status = 1 }],
            //SysMenus =
        };
        yield return new Role()
        {
            Name = "System Admin Group",
            IsEnable = true,
            Childrens = new List<Role>()
            {
                new Role() { Name = "Admin", IsEnable = true, SysUsers = [new SysUser() { UserName = "1", Password = "123", Email = "2@2.com", Status = 1 }],
                    SysMenus = SysMenuDataSeed().ToArray() }
            },
            SysUsers = [new SysUser() { UserName = "2", Password = "123", Email = "2@2.com", Status = 1 }]
        };
        yield return new Role()
        {
            Name = "Manage Group",
            IsEnable = true,
            Childrens = [
                new Role() { Name = "CEO", IsEnable = true, SysUsers = [new SysUser() { UserName = "3", Password = "123", Email = "3@3.com", Status = 1 }] }
            ],
            SysUsers = [new SysUser() { UserName = "4", Password = "123", Email = "4@4.com", Status = 1 }],
            //SysActions = SysActionDataSeed().ToArray(),
        };
        //yield return new Role() { Name = "Admin", IsEnable = true, ParentId = 2 };
        //yield return new Role() { Name = "CEO", IsEnable = true, ParentId = 3 };
    }

    public static IEnumerable<SysMenu> SysMenuDataSeed()
    {
        yield return new SysMenu()
        {
            Name = "系统管理",
            Childrens = new List<SysMenu>()
            {
                new SysMenu() { Name = "权限管理", LinkUrl = "/system/permission" },
                new SysMenu() { Name = "角色管理", LinkUrl = "/system/role", SysActions = SysActionDataSeed().ToArray()},
                new SysMenu() { Name = "操作管理", LinkUrl = "/system/action" },
                new SysMenu() { Name = "用户管理", LinkUrl = "/system/user" },
            }
        };
        yield return new SysMenu()
        {
            Name = "报表管理",
            Childrens = new List<SysMenu>()
            {
                new SysMenu() { Name = "表单", LinkUrl = "/form/form" }
            }
        };
    }

    public static IEnumerable<SysAction> SysActionDataSeed()
    {
        yield return new SysAction() { Name = "新增角色", Action = "Create", Contronller = "Role", ApiUrl = "/api/role/create" };
        yield return new SysAction() { Name = "修改角色", Action = "Update", Contronller = "Role", ApiUrl = "/api/role/update" };
        yield return new SysAction() { Name = "删除角色", Action = "Delete", Contronller = "Role", ApiUrl = "/api/role/delete" };
        yield return new SysAction() { Name = "查询角色", Action = "Get", Contronller = "Role", ApiUrl = "/api/role/get" };
    }

    public static IEnumerable<SysUserRole> SysUserRoleDataSeed()
    {
        yield return new SysUserRole() { UserId = 1, RoleId = 1 };
        yield return new SysUserRole() { UserId = 2, RoleId = 3 };
        yield return new SysUserRole() { UserId = 3, RoleId = 2 };
    }

    public static IEnumerable<RoleSysMenu> RoleSysMenuDataSeed()
    {
        yield return new RoleSysMenu() { RoleId = 1, SysMenuId = 1 };
        yield return new RoleSysMenu() { RoleId = 1, SysMenuId = 6 };
    }

    public static IEnumerable<RoleSysAction> RoleSysActionDataSeed()
    {
        yield return new RoleSysAction() { RoleId = 1, SysActionId = 1, SysMenuId = 3 };
        yield return new RoleSysAction() { RoleId = 1, SysActionId = 2, SysMenuId = 3 };
        yield return new RoleSysAction() { RoleId = 1, SysActionId = 3, SysMenuId = 3 };
        yield return new RoleSysAction() { RoleId = 1, SysActionId = 4, SysMenuId = 3 };
        yield return new RoleSysAction() { RoleId = 2, SysActionId = 1, SysMenuId = 3 };
        yield return new RoleSysAction() { RoleId = 2, SysActionId = 3, SysMenuId = 3 };
    }
}