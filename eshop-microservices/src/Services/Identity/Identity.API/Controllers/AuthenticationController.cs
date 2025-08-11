using Identity.API.Application.Commands.Authentication;
using Identity.API.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IMediator _mediator;

        public AuthenticationController(ILogger<AuthenticationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<SignInViewModel>> SignIn([FromBody] SignInCommand command)
        {
            return Ok(await _mediator.Send(command, HttpContext.RequestAborted));
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "authPolicy")]
        public new async Task<ActionResult> SignOut()
        {
            await _mediator.Send(new SignOutCommand() { Key = HttpContext.User.Identity!.Name! }, HttpContext.RequestAborted);
            return Ok();
        }
    }
}