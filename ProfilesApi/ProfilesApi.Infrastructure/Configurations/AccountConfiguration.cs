using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<AccountEntity>
{
    public void Configure(EntityTypeBuilder<AccountEntity> builder)
    {
        builder.ConfigureBaseEntity();
        builder.ConfigureSoftDelete();

        builder.Property(x => x.PhoneNumber).IsRequired();
        builder.Property(x => x.Email).IsRequired();
        builder.Property(x => x.HashedPassword).IsRequired();
        builder.Property(x => x.Role).IsRequired();

        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
    }
}

