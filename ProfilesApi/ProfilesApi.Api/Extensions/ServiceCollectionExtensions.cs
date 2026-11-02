using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using ProfilesApi.Api.Clients;
using ProfilesApi.Api.Middlewares;
using ProfilesApi.Api.Options;
using ProfilesApi.Application;
using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.DTOs.Responses.Registration;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Enum;
using ProfilesApi.Infrastructure;

namespace ProfilesApi.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLayerServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddInfrastructure(configuration);
        services.AddApplicationServices();
        services.AddAuthApiClient(configuration);

        return services;
    }

    public static IServiceCollection AddAuthApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<AuthApiOptions>()
            .Bind(configuration.GetSection(AuthApiOptions.SectionName))
            .Validate(options => Uri.TryCreate(options.BaseUrl, UriKind.Absolute, out _), "AuthApi:BaseUrl must be an absolute URI.")
            .ValidateOnStart();

        services.AddHttpClient<IIdentityClient, AuthIdentityClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<AuthApiOptions>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl.TrimEnd('/') + "/");
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }

    public static IServiceCollection AddApiControllers(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services, IConfiguration configuration)
    {
        var authority = KeycloakAuthority(configuration);
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new() { Title = "Profiles API", Version = "v1" });
            options.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri($"{authority}/.well-known/openid-configuration")
            });
            options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Keycloak", document)] = ["openid"]
            });
            options.UseOneOfForPolymorphism();
            options.SelectDiscriminatorNameUsing(type =>
                type == typeof(RegisterUserDto) || type == typeof(RegisteredUserDto) ? "role" : null);
            options.SelectDiscriminatorValueUsing(type => type.Name switch
            {
                nameof(RegisterPatientDto) or nameof(RegisteredPatientDto) => nameof(AccountRole.Patient),
                nameof(RegisterDoctorDto) or nameof(RegisteredDoctorDto) => nameof(AccountRole.Doctor),
                nameof(RegisterAdminDto) or nameof(RegisteredAdminDto) => nameof(AccountRole.Admin),
                nameof(RegisterReceptionistDto) or nameof(RegisteredReceptionistDto) => nameof(AccountRole.Receptionist),
                _ => null
            });
            options.SelectSubTypesUsing(type =>
            {
                if (type == typeof(RegisterUserDto))
                {
                    return
                    [
                        typeof(RegisterPatientDto),
                        typeof(RegisterDoctorDto),
                        typeof(RegisterAdminDto),
                        typeof(RegisterReceptionistDto)
                    ];
                }

                if (type == typeof(RegisteredUserDto))
                {
                    return
                    [
                        typeof(RegisteredPatientDto),
                        typeof(RegisteredDoctorDto),
                        typeof(RegisteredAdminDto),
                        typeof(RegisteredReceptionistDto)
                    ];
                }

                return [];
            });
        });

        return services;
    }

    private static string KeycloakAuthority(IConfiguration configuration)
    {
        var options = configuration.GetSection(KeycloakOptions.SectionName).Get<KeycloakOptions>() ?? new KeycloakOptions();
        if (string.IsNullOrWhiteSpace(options.BaseUrl) || string.IsNullOrWhiteSpace(options.Realm))
            throw new InvalidOperationException("Keycloak authentication settings are incomplete.");

        return $"{options.BaseUrl.TrimEnd('/')}/realms/{options.Realm}";
    }

    public static IServiceCollection AddExceptionHandler(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
