using Identity.Domain.Entities.Users;
using Identity.Infrastructure.Persistence.ValueConverters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Identity.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users", "identity");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(x => x.Email)
            .HasConversion<EmailValueConverter>()
            .HasColumnName("email")
            .HasMaxLength(254)
            .IsRequired();
    
        builder.HasIndex(x => x.Email)
            .HasDatabaseName("ux_users_email")
            .IsUnique();

    }
}