using System.Net;
using System.Security.Claims;
using Duende.IdentityModel;
using MicroServiceDemo.Infrastructure.Core.Provider;
using Microsoft.AspNetCore.Authentication;
using NetCommon.Core.Extensions;
using NetCommon.Redis;

namespace Identity.API.Extensions.AuthenticationExtensions;

public class CustomAuthenticationHandler : IAuthenticationSignInHandler
{
    private readonly ILogger<CustomAuthenticationHandler> _logger;
    private readonly IJwtProvider _jwtProvider;
    private readonly IRedisRepository _redisRepository;

    private HttpContext _httpContext;
    private AuthenticationScheme _scheme;

    public CustomAuthenticationHandler(ILogger<CustomAuthenticationHandler> logger, IJwtProvider jwtProvider, IRedisRepository redisRepository)
    {
        _logger = logger;
        _jwtProvider = jwtProvider;
        _redisRepository = redisRepository;
    }

    public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
    {
        _scheme = scheme;
        _httpContext = context;

        return Task.CompletedTask;
    }

    /// <summary>
    /// 认证逻辑
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<AuthenticateResult> AuthenticateAsync()
    {
        if (_httpContext.Request.Headers.Authorization.Count == 0)
        {
            return await Task.FromResult(AuthenticateResult.NoResult());
        }

        var clientToken = _httpContext.Request.Headers.Authorization.First()!.Split(" ")[1];

        var clientJsonWebToken = _jwtProvider.ParseJwt(clientToken);

        var serverToken = await _redisRepository.Get(clientJsonWebToken.GetClaim(JwtClaimTypes.Id).Value);

        if (serverToken.IsEmptyVal() || serverToken.Equals(clientToken))
        {
            return await Task.FromResult(AuthenticateResult.Fail("unauthorized!"));
        }

        var token = _jwtProvider.ParseJwt(serverToken);

        //AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity[]), null, ""));
        return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(new ClaimsIdentity(token.Claims)), null, CustomAuthenticationDefaults.AuthenticationScheme)));
    }

    public Task ChallengeAsync(AuthenticationProperties? properties)
    {
        _httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

        return Task.CompletedTask;
    }

    public Task ForbidAsync(AuthenticationProperties? properties)
    {
        _httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        return Task.CompletedTask;
    }

    public Task SignOutAsync(AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }
}