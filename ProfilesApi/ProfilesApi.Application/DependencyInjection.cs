using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Services;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace ProfilesApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        services.AddFluentValidationAutoValidation(configuration =>
        {
            configuration.EnableCustomBindingSourceAutomaticValidation = true;
            configuration.EnableNullBindingSourceAutomaticValidation = true;
        });

        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IReceptionistService, ReceptionistService>();
        services.AddScoped<IPatientService, PatientService>();

        return services;
    }
}
