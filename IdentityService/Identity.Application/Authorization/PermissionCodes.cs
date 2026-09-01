namespace Identity.Application.Authorization;

public static class PermissionCodes
{
    public static class Users
    {
        public static readonly PermissionDefinition Create =
            new PermissionDefinition("user:create", "Create User", "Allow Creating users");

        public static readonly PermissionDefinition Update =
            new PermissionDefinition("user:update", "Update User", "Allow Updating users");

        public static readonly PermissionDefinition Delete =
            new PermissionDefinition("user:delete", "Delete User", "Allow Deleting users");

        public static readonly PermissionDefinition View =
            new PermissionDefinition("user:view", "View User", "Allow Viewing users");
    }

    public static class Roles
    {
        public static readonly PermissionDefinition View =
            new PermissionDefinition("role:view", "View Role", "Allow Viewing roles");

        public static readonly PermissionDefinition Create =
            new PermissionDefinition("role:create", "Create Role", "Allow creating roles");

        public static readonly PermissionDefinition Update =
            new PermissionDefinition("role:update", "Update Role", "Allow Updating roles");

        public static readonly PermissionDefinition Delete =
            new PermissionDefinition("role:delete", "Delete Role", "Allow Deleting roles");
    }

    public static class Permissions
    {
        public static readonly PermissionDefinition View =
            new PermissionDefinition("permission:view", "View Role", "Allow Viewing roles");

        public static readonly PermissionDefinition Create =
            new PermissionDefinition("permission:create", "View User", "Allow Viewing users");

        public static readonly PermissionDefinition Update =
            new PermissionDefinition("permission:update", "Update Role", "Allow Updating roles");

        public static readonly PermissionDefinition Delete =
            new PermissionDefinition("permission:delete", "Delete Role", "Allow Deleting roles");
    }

    public static IReadOnlyCollection<PermissionDefinition> All =>
    [
        Users.Create,
        Users.Delete,
        Users.Update,
        Users.View,

        Roles.Create,
        Roles.Delete,
        Roles.Update,
        Roles.View,

        Permissions.View,
        Permissions.Delete,
        Permissions.Create,
        Permissions.Update
    ];
}