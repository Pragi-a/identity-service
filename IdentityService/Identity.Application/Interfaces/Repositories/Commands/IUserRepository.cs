using Identity.Application.Common.Results;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;

namespace Identity.Application.Interfaces.Repositories.Commands;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken);
    
    Task AddAsync(User user, CancellationToken cancellationToken);
    
    Task<Result> SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    
    Task DeleteAsync(User user, CancellationToken cancellationToken);
    
}