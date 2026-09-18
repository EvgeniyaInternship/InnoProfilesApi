using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Interfaces;
using ProfilesApi.Infrastructure.Repositories;

namespace ProfilesApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContextFactory<ProfilesDbContext>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IReceptionistRepository, ReceptionistRepository>();

        return services;
    }
}
