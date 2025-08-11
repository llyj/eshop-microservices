using Identity.Domain;
using Identity.Domain.Repositories;
using MicroServiceDemo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class RoleRepository : Repository<Role, int, IdentityDbContext>, IRoleRepository
{
    public RoleRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<Role>> GetRolesByRoleName(List<string> roleNames)
    {
        return await DbContext.Roles.Include(r => r.SysActions).Where(r => roleNames.Any(o => o == r.Name))
            .ToListAsync();
    }
}