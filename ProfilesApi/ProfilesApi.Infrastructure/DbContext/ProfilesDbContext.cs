using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProfilesApi.Infrastructure.Configurations;

namespace ProfilesApi.Infrastructure.ProfilesDbContext;

public class ProfilesDbContext(DbContextOptions<ProfilesDbContext> options, IConfiguration configuration) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
        modelBuilder.ApplyConfiguration(new AdminConfiguration());
        modelBuilder.ApplyConfiguration(new DoctorConfiguration());
        modelBuilder.ApplyConfiguration(new PatientConfiguration());
        modelBuilder.ApplyConfiguration(new ReceptionistConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("profiles-db"));
    }
}

