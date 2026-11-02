using Microsoft.AspNetCore.Authorization;
using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Api.Authorization;

public static class Policies
{
    public const string Admin = "Admin";
    public const string Doctor = "Doctor";
    public const string Receptionist = "Receptionist";
    public const string Staff = "Staff";
    public const string CreateAccount = "CreateAccount";
    public const string DoctorAndAdmin = "DoctorAndAdmin";
    public const string PatientAndAdmin = "PatientAndAdmin";
    public const string ReceptionistAndAdmin = "ReceptionistAndAdmin";

    public static void AddProfilePolicies(this AuthorizationOptions options)
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();

        options.AddPolicy(Admin, policy => policy.Require(AccountRole.Admin));
        options.AddPolicy(Doctor, policy => policy.Require(AccountRole.Doctor));
        options.AddPolicy(Receptionist, policy => policy.Require(AccountRole.Receptionist));
        options.AddPolicy(Staff, policy => policy.Require(AccountRole.Admin, AccountRole.Doctor, AccountRole.Receptionist));
        options.AddPolicy(CreateAccount, policy => policy.Require(AccountRole.Admin, AccountRole.Patient));
        options.AddPolicy(DoctorAndAdmin, policy => policy.Require(AccountRole.Admin, AccountRole.Doctor));
        options.AddPolicy(PatientAndAdmin, policy => policy.Require(AccountRole.Admin, AccountRole.Patient));
        options.AddPolicy(ReceptionistAndAdmin, policy => policy.Require(AccountRole.Admin, AccountRole.Receptionist));
    }

    private static void Require(this AuthorizationPolicyBuilder policy, params AccountRole[] roles)
    {
        policy.RequireRole([.. roles.Select(role => role.ToString())]);
    }
}
