using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using ProfilesApi.Api.Authorization;
using ProfilesApi.Api.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace ProfilesApi.Api.Extensions;

public static class AuthenticationExtensions
{
    private const string AccessTokenCookie = "accessToken";

    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(KeycloakOptions.SectionName).Get<KeycloakOptions>() ?? new KeycloakOptions();
        if (string.IsNullOrWhiteSpace(options.BaseUrl)
            || string.IsNullOrWhiteSpace(options.Realm)
            || string.IsNullOrWhiteSpace(options.Audience))
        {
            throw new InvalidOperationException("Keycloak authentication settings are incomplete.");
        }

        var authority = $"{options.BaseUrl.TrimEnd('/')}/realms/{options.Realm}";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = authority;
                jwtOptions.Audience = options.Audience;
                jwtOptions.RequireHttpsMetadata = options.RequireHttpsMetadata;
                jwtOptions.MapInboundClaims = false;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = options.Audience,
                    AudienceValidator = IsExpectedAudience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromSeconds(30),
                    NameClaimType = "preferred_username",
                    RoleClaimType = "role"
                };
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.TryGetValue(AccessTokenCookie, out var accessToken)
                            && !string.IsNullOrWhiteSpace(accessToken))
                        {
                            context.Token = accessToken;
                        }

                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        if (context.Principal?.Identity is ClaimsIdentity identity)
                            MapRoleClaims(identity);

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization(authorization => authorization.AddProfilePolicies());

        return services;
    }

    private static bool IsExpectedAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
    {
        var expectedAudience = validationParameters.ValidAudience;
        if (string.IsNullOrWhiteSpace(expectedAudience))
            return false;
        if (audiences?.Contains(expectedAudience, StringComparer.Ordinal) == true)
            return true;
        var authorizedParty = securityToken switch
        {
            JsonWebToken jsonWebToken when jsonWebToken.TryGetPayloadValue("azp", out string? party) => party,
            JwtSecurityToken jwtSecurityToken => jwtSecurityToken.Claims.FirstOrDefault(claim => claim.Type == "azp")?.Value,
            _ => null
        };
        return string.Equals(authorizedParty, expectedAudience, StringComparison.Ordinal);
    }

    private static void MapRoleClaims(ClaimsIdentity identity)
    {
        var roles = new HashSet<string>(StringComparer.Ordinal);
        var realmAccess = identity.FindFirst("realm_access")?.Value;
        if (!string.IsNullOrWhiteSpace(realmAccess))
        {
            try
            {
                using var document = JsonDocument.Parse(realmAccess);
                if (document.RootElement.TryGetProperty("roles", out var realmRoles)
                    && realmRoles.ValueKind == JsonValueKind.Array)
                {
                    foreach (var role in realmRoles.EnumerateArray())
                    {
                        var value = role.GetString();
                        if (!string.IsNullOrWhiteSpace(value))
                            roles.Add(value);
                    }
                }
            }
            catch (JsonException)
            {
            }
        }

        foreach (var role in roles)
        {
            if (!identity.HasClaim("role", role))
                identity.AddClaim(new Claim("role", role));
        }
    }
}
