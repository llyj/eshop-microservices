using System.Text.Json.Serialization;
using MicroServiceDemo.Domain;

namespace Identity.Domain;

public class SysUser : Entity<int>, IAggregateRoot
{
    public string UserName { get; set; }

    public string Password { get; set; }

    public string? PhoneNumber { get; set; }

    public string Email { get; set; }

    public int Status { get; set; }

    public string? Remark { get; set; }

    [JsonIgnore]
    public virtual ICollection<Role> Roles { get; set; }
}