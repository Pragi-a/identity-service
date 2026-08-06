namespace Identity.Domain.Entities;

public sealed class UserRole
{
    public Guid Id { get; private set; }
    
    public Guid RoleId { get; private set; }

    private UserRole(Guid Id, Guid roleId)
    {
        this.Id = Id;
        this.RoleId = roleId;
    }

    public static UserRole Create(Guid roleId)
    {
        if (roleId == Guid.Empty)
            throw new ArgumentException("Role Id cannot be empty");
        
        
        return new UserRole(Guid.NewGuid(), roleId);
    }
}