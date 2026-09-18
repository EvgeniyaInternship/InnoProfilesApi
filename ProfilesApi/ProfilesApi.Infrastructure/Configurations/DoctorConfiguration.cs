using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Configurations;

public class DoctorConfiguration : IEntityTypeConfiguration<DoctorEntity>
{
    public void Configure(EntityTypeBuilder<DoctorEntity> builder)
    {
        builder.ConfigureBaseEntity();
        builder.ConfigureSoftDelete();
        builder.ConfigurePerson();

        builder.Property(x => x.SpecializationId).IsRequired();
        builder.HasIndex(x => x.SpecializationId);

        builder.Property(x => x.OfficeId).IsRequired();
        builder.HasIndex(x => x.OfficeId);
    }
}
