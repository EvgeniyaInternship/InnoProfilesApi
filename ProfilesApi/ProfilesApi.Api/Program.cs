using ProfilesApi.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLayerServices(builder.Configuration);

builder.Services.AddApiControllers();
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

app.UseSwaggerDocumentation();
app.UseApiMiddlewares();

app.Run();
