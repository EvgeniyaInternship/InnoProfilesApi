using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Repositories;

namespace ProfilesApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ProfilesDbContext>();

        services.AddScoped<IGenericRepository<AccountEntity>, AccountRepository>();
        services.AddScoped<IGenericRepository<AdminEntity>, AdminRepository>();
        services.AddScoped<IGenericRepository<DoctorEntity>, DoctorRepository>();
        services.AddScoped<IGenericRepository<PatientEntity>, PatientRepository>();
        services.AddScoped<IGenericRepository<ReceptionistEntity>, ReceptionistRepository>();

        return services;
    }
}
