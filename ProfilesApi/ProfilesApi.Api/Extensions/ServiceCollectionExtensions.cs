using ProfilesApi.Application;

namespace ProfilesApi.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLayerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices(configuration);

        return services;
    }

    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Profiles API", Version = "v1" });
        });

        return services;
    }
}
