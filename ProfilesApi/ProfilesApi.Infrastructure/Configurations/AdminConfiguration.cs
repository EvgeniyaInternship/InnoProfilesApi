using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Configurations;

public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
{
    public void Configure(EntityTypeBuilder<AdminEntity> builder)
    {
        builder.ConfigureBaseEntity();
        builder.ConfigureSoftDelete();
        builder.ConfigurePerson();

        builder.Property(x => x.OfficeId).IsRequired();
        builder.HasIndex(x => x.OfficeId);
    }
}
