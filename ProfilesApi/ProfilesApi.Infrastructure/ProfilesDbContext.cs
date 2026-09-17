using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Configurations;

namespace ProfilesApi.Infrastructure;

public class ProfilesDbContext(DbContextOptions<ProfilesDbContext> options, IConfiguration configuration) : DbContext(options)
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
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("profiles-db"));
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        ConvertDeletesToSoftDeletes();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDeletesToSoftDeletes()
    {
        foreach (var entry in ChangeTracker.Entries<SoftDeletableEntity>())
        {
            if (entry.State != EntityState.Deleted)
                continue;

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedOnUtc = DateTime.UtcNow;
        }
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}

