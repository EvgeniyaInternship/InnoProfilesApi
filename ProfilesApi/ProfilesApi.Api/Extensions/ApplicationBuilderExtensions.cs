using ProfilesApi.Api.Options;

namespace ProfilesApi.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var keycloak = app.Configuration.GetSection(KeycloakOptions.SectionName).Get<KeycloakOptions>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles API v1");
                c.OAuthClientId(keycloak?.Audience);
                c.OAuthUsePkce();
                c.OAuthScopes("openid");
            });
        }

        return app;
    }

    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
