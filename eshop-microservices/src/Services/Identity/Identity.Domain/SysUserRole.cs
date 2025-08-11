using System.Text.Json.Serialization;
using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class SysUserRole : Entity<int>
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    [JsonIgnore]
    public SysUser User { get; set; }

    [JsonIgnore]
    public Role Role { get; set; }
}