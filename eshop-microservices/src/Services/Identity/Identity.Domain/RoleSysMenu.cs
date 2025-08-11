using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class RoleSysMenu : Entity<int>
{
    public int RoleId { get; set; }

    public int SysMenuId { get; set; }

    public Role Role { get; set; }

    public SysMenu SysMenu { get; set; }
}