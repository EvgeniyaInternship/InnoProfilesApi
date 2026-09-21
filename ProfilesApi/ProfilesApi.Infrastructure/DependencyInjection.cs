using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Infrastructure.Context;
using ProfilesApi.Infrastructure.Data;
using ProfilesApi.Infrastructure.Interfaces;
using ProfilesApi.Infrastructure.Repositories;
using System.Reflection;

namespace ProfilesApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .Validate(options => !string.IsNullOrWhiteSpace(options.ProfilesDb))
            .ValidateOnStart();

        var interceptorTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => typeof(IInterceptor).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface);
        
        foreach (var interceptorType in interceptorTypes)
        {
            services.AddScoped(interceptorType);
        }

        services.AddDbContextFactory<ProfilesDbContext>();

        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IReceptionistRepository, ReceptionistRepository>();

        return services;
    }
}