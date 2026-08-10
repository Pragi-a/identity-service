using Identity.Application.Common.Errors;
using Identity.Application.Common.Results;
using Identity.Application.Interfaces.Repositories.Commands;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Repositories;

public sealed class PermissionRepository(IdentityDbContext context) : IPermissionRepository
{
    public async Task AddAsync(Permission permission, CancellationToken cancellationToken)
    {
        await context.Permissions.AddAsync(permission, cancellationToken);
    }

    public async Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await context.Permissions
            .FirstOrDefaultAsync(p => p.Code == code, cancellationToken);
    }

    public async Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Permissions
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Result> SaveChangesAsync(CancellationToken cancellationToken)
    {
        try
        {
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            return Result.Failure(CommonErrors.ConcurrencyConflict);
        }
    }
}