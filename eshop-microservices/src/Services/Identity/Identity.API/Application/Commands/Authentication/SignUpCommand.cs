using Identity.API.Application.Models;
using Identity.Domain.Repositories;
using MediatR;
using MicroServiceDemo.Infrastructure.Core.Provider;

namespace Identity.API.Application.Commands.Authentication;

public class SignUpCommand : IRequest<SignUpViewModel>
{
}

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpViewModel>
{
    private readonly ILogger<SignInCommandHandler> _logger;
    private readonly ISysUserRepository _sysUserRepository;
    private readonly IJwtProvider _jwtProvider;

    public async Task<SignUpViewModel> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        return new SignUpViewModel();
    }
}