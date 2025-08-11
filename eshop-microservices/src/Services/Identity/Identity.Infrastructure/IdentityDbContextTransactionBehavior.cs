using MicroServiceDemo.Infrastructure.Core.Behaviors;
using Microsoft.Extensions.Logging;

namespace Identity.Infrastructure;

public class IdentityDbContextTransactionBehavior<TRequest, TResponse> : TransactionBehavior<IdentityDbContext, TRequest, TResponse>
{
    public IdentityDbContextTransactionBehavior(ILogger<IdentityDbContextTransactionBehavior<TRequest, TResponse>> logger, IdentityDbContext dbContext) : base(logger, dbContext)
    {
    }
}