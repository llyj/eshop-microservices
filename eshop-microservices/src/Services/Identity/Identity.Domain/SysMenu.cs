using System.Text.Json.Serialization;
using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class SysMenu : Entity<int>
{
    public string Name { get; set; }

    /// <summary>
    /// 菜单地址
    /// </summary>
    public string? LinkUrl { get; set; }

    /// <summary>
    /// 菜单描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 序号
    /// </summary>
    public int? SortBy { get; set; }

    public int? ParentId { get; set; }

    [JsonIgnore]
    public SysMenu Parent { get; set; }

    [JsonIgnore]
    public virtual ICollection<SysMenu> Childrens { get; set; }

    [JsonIgnore]
    public virtual ICollection<SysAction> SysActions { get; set; }

    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; }
}