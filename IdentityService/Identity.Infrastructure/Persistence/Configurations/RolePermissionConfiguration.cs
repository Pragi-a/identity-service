using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable("role_permissions","auth");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_role_permissions");

        builder.Property(x => x.Id)
            .HasColumnName("role_permission_id")
            .IsRequired();
        
        builder.Property(x => x.PermissionId)
            .HasColumnName("permission_id")
            .IsRequired();
        
        builder.Property<Guid>("RoleId")
            .HasColumnName("role_id")
            .IsRequired();
        
        builder.HasIndex("RoleId",nameof(RolePermission.PermissionId))
            .IsUnique()
            .HasDatabaseName("ux_role_permissions_roleid_permissionid");;
    }
}