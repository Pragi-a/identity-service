using Identity.Domain.Entities.Users;
using Identity.Domain.ValueObjects;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
    Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken);
    
    Task AddAsync(User user, CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken);
    
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken);
    
}