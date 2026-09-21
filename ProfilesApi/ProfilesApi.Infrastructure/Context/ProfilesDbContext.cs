using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Data;

namespace ProfilesApi.Infrastructure.Context;

public class ProfilesDbContext(IOptions<DatabaseOptions> dbOptions, 
                               IEnumerable<IInterceptor> interceptors) : DbContext()
{
    public DbSet<AccountEntity> Accounts { get; set; }
    public DbSet<AdminEntity> Admins { get; set; }
    public DbSet<DoctorEntity> Doctors { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<ReceptionistEntity> Receptionists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfilesDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .UseNpgsql(dbOptions.Value.ProfilesDb)
            .AddInterceptors(interceptors);
    }
}

