using Identity.Application.Authorization;
using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Seed;

public sealed class DatabaseSeeder(IdentityDbContext context)
{
    public async Task SeedAsync()
    {
        await SeedPermissionsAsync();
        await SeedRolesAsync();
        await SeedRolePermissionAsync();
    }

    private async Task SeedPermissionsAsync()
    {
        var existingPermissions = await context.Permissions
            .Select(x => x.Code)
            .Distinct()
            .ToListAsync();

        var permissionsToAdd = PermissionCodes.All
            .Where(p => !existingPermissions.Contains(p.Code))
            .Select(p => Permission.Create(
                p.Code,
                p.DisplayName,
                p.Description))
            .ToList();

        if (permissionsToAdd.Count > 0)
        {
            await context.Permissions.AddRangeAsync(permissionsToAdd);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedRolesAsync()
    {
        var existingRoles = await context.Roles.Select(x => x.Name)
            .Distinct()
            .ToListAsync();

        var rolesToAdd = RoleDefinitions.All
            .Where(x => !existingRoles.Contains(x.Name))
            .Select(r => Role.Create(r.Name, r.Description))
            .ToList();
        
        if (rolesToAdd.Count > 0)
        {
            await context.Roles.AddRangeAsync(rolesToAdd);
            await context.SaveChangesAsync();
        }
    }

    private async Task SeedRolePermissionAsync()
    {
        var adminRole = await context.Roles
                            .Include(x => x.Permissions)
                            .FirstAsync(x => x.Name == RoleDefinitions.Admin.Name);
        
        var permissionIds = await context.Permissions.Select(x => x.Id).ToListAsync();
        
        adminRole.ReplacePermissions(permissionIds);
        
            
        foreach (var entry in context.ChangeTracker.Entries())
        {
            Console.WriteLine(
                $"{entry.Entity.GetType().Name} - {entry.State}");
        }
        await context.SaveChangesAsync();
        
    }
}