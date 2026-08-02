using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities.Users;

public class User
{
    public Guid Id { get; init; }
    public Email Email { get; init; }
    public string PasswordHash { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedAt { get; init; }

    public User(Email email, string passwordHash, string firstName, string lastName)
    {
        Id = Guid.NewGuid();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;

        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
    }
}