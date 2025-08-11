using Identity.Domain.Repositories;
using Identity.Infrastructure.Repositories;
using MicroServiceDemo.Infrastructure.Core.Options;
using MicroServiceDemo.Infrastructure.Core.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtConfig(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddSingleton<IJwtProvider, JwtProvider>();
        service.Configure<JwtOption>(configuration);

        return service;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection service)
    {
        service.AddScoped<ISysUserRepository, SysUserRepository>();
        service.AddScoped<IRoleRepository, RoleRepository>();
        return service;
    }
}