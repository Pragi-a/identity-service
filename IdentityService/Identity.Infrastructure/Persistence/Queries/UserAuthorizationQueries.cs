using Identity.Application.Interfaces.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Queries;

public sealed class UserAuthorizationQueries(IdentityDbContext context) : IUserAuthorizationQueries
{
    public async Task<IReadOnlyCollection<string>> GetPermissionCodesAsync(Guid userId,
        CancellationToken cancellationToken)
    {
        return await context.UserRoles
            .AsNoTracking()
            .Where(x => EF.Property<Guid>(x, "UserId") == userId)
            .Join(
                context.RolePermissions,
                ur => ur.RoleId,
                rp => EF.Property<Guid>(rp, "RoleId"),
                (ur, rp) => rp.PermissionId
            ).Join(
                context.Permissions,
                res => res,
                p => p.Id,
                (res, p) => p.Code
            )
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}