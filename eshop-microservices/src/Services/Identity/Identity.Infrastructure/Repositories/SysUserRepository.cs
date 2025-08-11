using Identity.Domain;
using Identity.Domain.Repositories;
using MicroServiceDemo.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories;

public class SysUserRepository : Repository<SysUser, int, IdentityDbContext>, ISysUserRepository
{
    public SysUserRepository(IdentityDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<SysUser?> GetByUserNameAsync(string userName)
    {
        var user = await DbContext.SysUsers
            .Include(o => o.Roles)
            .SingleOrDefaultAsync(o => o.UserName == userName);

        //if (user != null)
        //{
        //    await DbContext.IdentityUsers.Entry(user).Collection(o => o.Roles).LoadAsync();
        //}

        return user;
    }
}