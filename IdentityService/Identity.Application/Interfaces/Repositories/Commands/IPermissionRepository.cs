using Identity.Application.Common.Results;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Repositories.Commands;

public interface IPermissionRepository 
{
    Task AddAsync(Permission permission, CancellationToken cancellationToken);
    
    Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken);
    
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken);
    
}