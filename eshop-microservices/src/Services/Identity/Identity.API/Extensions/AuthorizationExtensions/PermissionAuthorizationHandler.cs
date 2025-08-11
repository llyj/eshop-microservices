using Duende.IdentityModel;
using Identity.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Identity.API.Extensions.AuthorizationExtensions;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private readonly IRoleRepository _roleRepository;

    public PermissionAuthorizationHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
    {
        // 没有角色直接失败
        if (!context.User.HasClaim(c => c.Type == JwtClaimTypes.Role))
        {
            context.Fail();
            return;
        }

        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }
        else
        {
            var roleNames = context.User.Claims.Where(c => c.Type == JwtClaimTypes.Role).Select(c => c.Value).ToList();
            var roles = await _roleRepository.GetRolesByRoleName(roleNames);

            //switch (context.Resource)
            //{
            //    case HttpContext httpContext:
            //        var authorizeData = httpContext.GetEndpoint()?.Metadata.GetMetadata<IAuthorizeData>();
            //        break;

            //    case Endpoint endpoint:
            //        var metadata = endpoint.Metadata.GetMetadata<IAuthorizeData>();
            //        break;
            //}

            ControllerActionDescriptor controllerActionDescriptor = null;
            if (context.Resource is HttpContext)
            {
                var contextResource = (context.Resource as HttpContext).GetEndpoint();
                controllerActionDescriptor = contextResource.Metadata.GetMetadata<ControllerActionDescriptor>();
            }
            else
            {
                var contextResource = context.Resource as Endpoint;
                controllerActionDescriptor = contextResource.Metadata.GetMetadata<ControllerActionDescriptor>();
            }

            if (controllerActionDescriptor != null && roles.SelectMany(r => r.SysActions).Any(s => s.Contronller.Equals(controllerActionDescriptor.ControllerName, StringComparison.OrdinalIgnoreCase) && s.Action.Equals(controllerActionDescriptor.ActionName, StringComparison.OrdinalIgnoreCase)))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}