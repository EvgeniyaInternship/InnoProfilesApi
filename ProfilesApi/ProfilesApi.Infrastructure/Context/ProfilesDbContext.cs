using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Configurations;
using ProfilesApi.Infrastructure.Data;
using ProfilesApi.Infrastructure.Interceptors;

namespace ProfilesApi.Infrastructure.Context;

public class ProfilesDbContext(IOptions<DatabaseOptions> dbOptions) : DbContext()
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AdminEntity> Admins { get; set; }
    public DbSet<DoctorEntity> Doctors { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<ReceptionistEntity> Receptionists { get; set; }

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
        optionsBuilder
            .UseNpgsql(dbOptions.Value.ProfilesDb)
            .AddInterceptors(new TimestampInterceptor(), new SoftDeleteInterceptor());
    }
}

