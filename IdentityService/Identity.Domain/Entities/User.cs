using Identity.Domain.ValueObjects;

namespace Identity.Domain.Entities;

public sealed class User
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private readonly List<UserRole> _roles = [];

    public IReadOnlyCollection<UserRole> Roles => _roles;


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

    public void AddRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new ArgumentException("RoleId should not be empty");

        if (_roles.Any(x => x.RoleId == roleId))
            return;

        _roles.Add(UserRole.Create(roleId));
    }

    public void RemoveRole(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new ArgumentException("RoleId should not be empty");

        var userRole = _roles.FirstOrDefault(x => x.RoleId == roleId);

        if (userRole == null)
            return;

        _roles.Remove(userRole);
    }

    public bool HasRole(Guid roleId)
    {
        return _roles.Any(x => x.RoleId == roleId);
    }

    public void ReplaceRoles(IEnumerable<Guid> roleIds)
    {
        ArgumentNullException.ThrowIfNull(roleIds);
        _roles.Clear();

        var roles = roleIds.Distinct();

        foreach (var role in roles)
        {
            AddRole(role);
        }
    }
}