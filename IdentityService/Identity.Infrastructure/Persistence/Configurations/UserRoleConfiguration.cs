using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("user_roles", "auth");
        
        builder.HasKey(x => x.Id)
            .HasName("pk_user_roles");

        builder.Property(x => x.Id)
            .HasColumnName("user_role_id")
            .ValueGeneratedNever()
            .IsRequired();
        
        builder.Property(x => x.RoleId)
            .HasColumnName("role_id")
            .IsRequired();
        
        builder.Property<Guid>("UserId")
            .HasColumnName("user_id")
            .IsRequired();

        builder.HasOne<Role>()
            .WithMany()
            .HasForeignKey(x => x.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasIndex("UserId",nameof(UserRole.RoleId))
            .HasDatabaseName("ux_user_roles_user_id_role_id")
            .IsUnique();
    }
}