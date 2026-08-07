using Identity.Application.Authorization;
using Identity.Application.Interfaces.Security;
using Identity.Domain.Entities;
using Identity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Persistence.Seed;

public sealed class DatabaseSeeder(IdentityDbContext context, IPasswordHasher passwordHasher)
{
    public async Task SeedAsync()
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await SeedPermissionsAsync();
            await SeedRolesAsync();
            await SeedRolePermissionAsync();
            await SeedAdminUserAsync();

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
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

    private async Task SeedAdminUserAsync()
    {
        var isAdminPresent = await context.Users
            .AsNoTracking()
            .AnyAsync(x => x.Email == new Email("admin@amazonclone.local"));

        if (isAdminPresent) return;


        var adminRoleId = await context.Roles
            .AsNoTracking()
            .Where(x => x.Name == RoleDefinitions.Admin.Name)
            .Select(x => x.Id)
            .FirstAsync();
        

        var passwordHash = passwordHasher.HashPassword("Admin@123456");

        var email = new Email("admin@amazonclone.local");
        var user = new User(email, passwordHash, "Admin", "Admin");
        user.AddRole(adminRoleId);

        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }
}