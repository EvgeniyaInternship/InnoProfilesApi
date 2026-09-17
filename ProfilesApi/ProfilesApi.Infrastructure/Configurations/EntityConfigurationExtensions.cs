using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Configurations;

public static class EntityConfigurationExtensions
{
    public static void ConfigureBaseEntity<T>(this EntityTypeBuilder<T> builder) where T : BaseEntity
    {
        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder
            .Property(x => x.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }

    public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> builder) where T : SoftDeletableEntity
    {
        builder.HasQueryFilter(x => !x.IsDeleted);

        builder
            .HasIndex(x => x.IsDeleted)
            .HasFilter("\"IsDeleted\" = false");
    }

    public static void ConfigurePerson<T>(this EntityTypeBuilder<T> builder) where T : PersonEntity
    {
        builder.Property(x => x.FirstName).IsRequired();
        builder.Property(x => x.LastName).IsRequired();

        builder.HasIndex(x => x.AccountId).IsUnique();
    }
}
