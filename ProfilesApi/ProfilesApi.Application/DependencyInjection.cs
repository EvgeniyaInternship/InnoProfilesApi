using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Services;
using System.Reflection;

namespace ProfilesApi.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IReceptionistService, ReceptionistService>();
        services.AddScoped<IPatientService, PatientService>();

        return services;
    }
}
