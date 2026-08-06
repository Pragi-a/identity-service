using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("roles","auth");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_roles");

        builder.Property(x => x.Id)
            .HasColumnName("role_id")
            .IsRequired();
        
        builder.Property(x => x.Name)
            .HasColumnName("name")
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasMaxLength(256)
            .IsRequired();
        
        builder.HasMany(x => x.Permissions)
            .WithOne()
            .HasForeignKey("RoleId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Permissions)
            .HasField("_permissions")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        
        builder.HasIndex(x => x.Name)
            .IsUnique()
            .HasDatabaseName("ux_roles_name");

    }
}