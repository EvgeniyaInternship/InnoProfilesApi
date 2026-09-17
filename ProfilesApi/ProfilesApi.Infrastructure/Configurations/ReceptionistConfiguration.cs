using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Infrastructure.Configurations;

public class ReceptionistConfiguration : IEntityTypeConfiguration<ReceptionistEntity>
{
    public void Configure(EntityTypeBuilder<ReceptionistEntity> builder)
    {
        builder.ConfigureBaseEntity();
        builder.ConfigureSoftDelete();
        builder.ConfigurePerson();

        builder.Property(x => x.WorkStartDate).IsRequired();
        builder.Property(x => x.OfficeId).IsRequired();
        builder.HasIndex(x => x.OfficeId);
    }
}
