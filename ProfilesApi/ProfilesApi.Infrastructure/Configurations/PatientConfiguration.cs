using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<PatientEntity>
{
    public void Configure(EntityTypeBuilder<PatientEntity> builder)
    {
        builder.ConfigureBaseEntity();
        builder.ConfigureSoftDelete();
        builder.ConfigurePerson();

        builder.Property(x => x.InsuranceNumber).IsRequired();
    }
}
