namespace ProfilesApi.Api.Extensions;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseSwaggerDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Profiles API v1");
            });
        }

        return app;
    }

    public static WebApplication UseApiMiddlewares(this WebApplication app)
    {
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        return app;
    }
}
