namespace Identity.Domain.Entities;

public sealed class RolePermission
{
    public Guid Id { get; private set; }

    public Guid PermissionId { get; private set; }


    private RolePermission(Guid id, Guid permissionId)
    {
        Id = id;
        PermissionId = permissionId;
    }

    public static RolePermission Create(Guid permissionId)
    {
        if (permissionId == Guid.Empty)
            throw new ArgumentException("PermissionId Cannot be Empty");

        return new RolePermission(Guid.NewGuid(), permissionId);
    }
}