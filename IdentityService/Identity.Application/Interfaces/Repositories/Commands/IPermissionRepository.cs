using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Repositories.Commands;

public interface IPermissionRepository 
{
    Task AddAsync(Permission permission, CancellationToken cancellationToken);
    
    Task<Permission?> GetByCodeAsync(string code, CancellationToken cancellationToken);
    
    Task<Permission?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
}