using Identity.API.Extensions.AuthorizationExtensions;
using Identity.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(PermissionAuthorization.AuthPolicy)]
    public class RoleController : ControllerBase
    {
        [HttpGet("[action]")]
        public async Task<ActionResult<Role>> Get()
        {
            return Ok(await Task.FromResult(new Role()));
        }
    }
}