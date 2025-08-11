using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class RoleSysAction : Entity<int>
{
    public int RoleId { get; set; }

    public int SysActionId { get; set; }

    public int SysMenuId { get; set; }

    public Role Role { get; set; }

    public SysAction SysAction { get; set; }

    public SysMenu SysMenu { get; set; }
}