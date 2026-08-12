using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class RoleRepository(IdentityDbContext context) : IRoleRepository
{
    public async Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await context.Roles
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(r => r.Name == name, cancellationToken);
    }

    public async Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        return await context.Roles
            .Include(x => x.Permissions)
            .FirstOrDefaultAsync(r => r.Id == roleId, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Role>> GetByIdsAsync(IReadOnlyCollection<Guid> roleIds,
        CancellationToken cancellationToken)
    {
        return await context.Roles
            .Where(x => roleIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
    }


    public async Task AddAsync(Role role, CancellationToken cancellationToken)
    {
        await context.Roles.AddAsync(role, cancellationToken);
    }


}