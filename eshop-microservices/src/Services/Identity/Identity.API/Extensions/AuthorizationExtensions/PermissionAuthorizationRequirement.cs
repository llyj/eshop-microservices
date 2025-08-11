using Microsoft.AspNetCore.Authorization;

namespace Identity.API.Extensions.AuthorizationExtensions;

public class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    public string Name { get; set; }
}