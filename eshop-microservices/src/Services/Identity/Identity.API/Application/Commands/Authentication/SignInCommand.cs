using Duende.IdentityModel;
using Identity.API.Application.Models;
using Identity.Domain.Repositories;
using MediatR;
using MicroServiceDemo.Infrastructure.Core.Options;
using MicroServiceDemo.Infrastructure.Core.Provider;
using Microsoft.Extensions.Options;
using NetCommon.Redis;
using System.Security.Claims;

namespace Identity.API.Application.Commands.Authentication;

public class SignInCommand : IRequest<SignInViewModel>
{
    public string UserName { get; set; }

    public string Password { get; set; }
}

public class SignInCommandHandler : IRequestHandler<SignInCommand, SignInViewModel>
{
    private readonly ILogger<SignInCommandHandler> _logger;
    private readonly ISysUserRepository _sysUserRepository;
    private readonly IJwtProvider _jwtProvider;
    private readonly JwtOption _jwtOption;
    private readonly IRedisRepository _redisRepository;

    public SignInCommandHandler(ILogger<SignInCommandHandler> logger, ISysUserRepository sysUserRepository, IJwtProvider jwtProvider, IOptions<JwtOption> jwtOption, IRedisRepository redisRepository)
    {
        _logger = logger;
        _sysUserRepository = sysUserRepository;
        _jwtProvider = jwtProvider;
        _redisRepository = redisRepository;
        _jwtOption = jwtOption.Value;
    }

    public async Task<SignInViewModel> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _sysUserRepository.GetByUserNameAsync(request.UserName);

        if (user is null || user.Password != request.Password)
        {
            throw new Exception("账号或密码错误！");
        }

        var claims = new List<Claim>();
        if (user.Roles.Count > 0)
        {
            //claims.Add(new Claim(JwtClaimTypes.Role, string.Join(',', user.Roles.Select(r => r.Name))));
            claims.AddRange(user.Roles.Select(r => new Claim(JwtClaimTypes.Role, r.Name)));
        }

        var token = _jwtProvider.GenerateJwt(user.Id, claims);

        var signInViewModel = new SignInViewModel()
        {
            AccessToken = token,
            ExpireAt = _jwtOption.ExpireAt
        };

        await _redisRepository.Set(user.Id.ToString(), signInViewModel, TimeSpan.FromSeconds(_jwtOption.ExpireAt));

        return signInViewModel;
    }
}