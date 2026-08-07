namespace Identity.Application.Authorization;

public sealed class RoleDefinitions
{
    public static readonly RoleDefinition Admin = new("Admin", "Admin Role");

    public static IReadOnlyCollection<RoleDefinition> All => [Admin];
}