using MicroServiceDemo.Domain.Abstractions;

namespace Identity.Domain.Repositories;

public interface IRoleRepository : IRepository<Role, int>
{
    Task<List<Role>> GetRolesByRoleName(List<string> roleNames);
}