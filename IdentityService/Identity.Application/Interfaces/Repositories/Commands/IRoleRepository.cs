using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Repositories.Commands;

public interface IRoleRepository
{
    Task<Role?> GetByNameAsync(string name,  CancellationToken cancellationToken);
    
    Task<Role?> GetByIdAsync(Guid roleId, CancellationToken cancellationToken);
    
    Task<IReadOnlyCollection<Role>> GetByIdsAsync(IReadOnlyCollection<Guid> roleIds, CancellationToken cancellationToken);
    
    Task AddAsync(Role role, CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}