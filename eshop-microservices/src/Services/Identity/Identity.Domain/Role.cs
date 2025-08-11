using System.Text.Json.Serialization;
using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class Role : Entity<int>, IAggregateRoot
{
    public string Name { get; set; }

    public bool IsEnable { get; set; }

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    [JsonIgnore]
    public Role? Parent { get; set; }

    [JsonIgnore]
    public virtual ICollection<Role> Childrens { get; set; }

    [JsonIgnore]
    public virtual ICollection<SysUser> SysUsers { get; set; }

    [JsonIgnore]
    public virtual ICollection<SysMenu> SysMenus { get; set; }

    [JsonIgnore]
    public virtual ICollection<SysAction> SysActions { get; set; }

    //[JsonIgnore]
    //public virtual ICollection<RoleSysMenu> RoleSysMenus { get; set; }
}