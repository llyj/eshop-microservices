using MicroServiceDemo.Domain.Abstractions;

namespace Identity.Domain.Repositories;

public interface ISysUserRepository : IRepository<SysUser, int>
{
    Task<SysUser?> GetByUserNameAsync(string userName);
}