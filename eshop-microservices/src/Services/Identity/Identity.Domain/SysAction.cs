using System.Text.Json.Serialization;
using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class SysAction : Entity<int>
{
    public string Name { get; set; }

    public string ApiUrl { get; set; }

    public string Contronller { get; set; }

    public string Action { get; set; }

    public int SysMenuId { get; set; }

    public SysMenu SysMenu { get; set; }

    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; }
}