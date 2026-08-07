namespace Identity.Domain.Entities;

public sealed class Role
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Description { get; private set; }

    private readonly List<RolePermission> _permissions = [];

    public IReadOnlyCollection<RolePermission> Permissions => _permissions;


    private Role(Guid id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public static Role Create(string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Role name cannot be empty");

        name = name.Trim();

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Role description cannot be empty");

        description = description.Trim();

        return new Role(Guid.NewGuid(), name, description);
    }

    public void AddPermission(Guid permissionId)
    {
        if (permissionId == Guid.Empty)
            throw new ArgumentException("Permission ID cannot be empty");

        if (_permissions.Any(x => x.PermissionId == permissionId)) return;


        _permissions.Add(RolePermission.Create(permissionId));
    }

    public void RemovePermission(Guid permissionId)
    {
        var rolePermission = _permissions.FirstOrDefault(x => x.PermissionId == permissionId);

        if (rolePermission == null)
            return;
        _permissions.Remove(rolePermission);
    }

    public bool HasPermission(Guid permissionId)
    {
        return _permissions.Any(x => x.PermissionId == permissionId);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Role name cannot be empty");
        newName = newName.Trim();
        Name = newName;
    }

    public void UpdateDescription(string newDescription)
    {
        if (string.IsNullOrWhiteSpace(newDescription))
            throw new ArgumentException("Role description cannot be empty");
        newDescription = newDescription.Trim();
        Description = newDescription;
    }

    public void ReplacePermissions(IEnumerable<Guid> permissionIds)
    {
        ArgumentNullException.ThrowIfNull(permissionIds);
        _permissions.Clear();
        
        var distinctPermissionIds = permissionIds.Distinct();
        foreach (var permissionId in distinctPermissionIds)
        {
            AddPermission(permissionId);
        }
    }
}