using MediatR;
using NetCommon.Redis;

namespace Identity.API.Application.Commands.Authentication;

public class SignOutCommand : IRequest
{
    public string Key { get; set; }
}

public class SignOutCommandHandler : IRequestHandler<SignOutCommand>
{
    private readonly ILogger<SignOutCommandHandler> _logger;
    private readonly IRedisRepository _redisRepository;

    public SignOutCommandHandler(ILogger<SignOutCommandHandler> logger, IRedisRepository redisRepository)
    {
        _logger = logger;
        _redisRepository = redisRepository;
    }

    public async Task Handle(SignOutCommand request, CancellationToken cancellationToken)
    {
        await _redisRepository.RemoveAsync(request.Key);
    }
}