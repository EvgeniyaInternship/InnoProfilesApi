using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<ProfilesDbContext>();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }
}
