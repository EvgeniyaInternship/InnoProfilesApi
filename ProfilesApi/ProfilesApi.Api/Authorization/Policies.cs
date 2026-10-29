using Microsoft.AspNetCore.Authorization;
using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Api.Authorization;

public static class Policies
{
    public const string Admin = nameof(Admin);
    public const string Staff = nameof(Staff);
    public const string AnyRole = nameof(AnyRole);
    public const string ReadDoctors = nameof(ReadDoctors);
    public const string UpdateDoctor = nameof(UpdateDoctor);
    public const string UpdatePatient = nameof(UpdatePatient);
    public const string UpdateReceptionist = nameof(UpdateReceptionist);

    public static void AddProfilePolicies(this AuthorizationOptions options)
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        options.AddPolicy(Admin, policy => policy.Require(AccountRole.Admin));
        options.AddPolicy(Staff, policy => policy.Require(AccountRole.Admin, AccountRole.Doctor, AccountRole.Receptionist));
        options.AddPolicy(AnyRole, policy => policy.Require(AccountRole.Admin, AccountRole.Doctor, AccountRole.Receptionist, AccountRole.Patient));
        options.AddPolicy(ReadDoctors, policy => policy.Require(AccountRole.Admin, AccountRole.Doctor, AccountRole.Receptionist, AccountRole.Patient));
        options.AddPolicy(UpdateDoctor, policy => policy.Require(AccountRole.Admin, AccountRole.Doctor));
        options.AddPolicy(UpdatePatient, policy => policy.Require(AccountRole.Admin, AccountRole.Patient));
        options.AddPolicy(UpdateReceptionist, policy => policy.Require(AccountRole.Admin, AccountRole.Receptionist));
    }

    private static void Require(this AuthorizationPolicyBuilder policy, params AccountRole[] roles)
    {
        policy.RequireRole([.. roles.Select(role => role.ToString())]);
    }
}
