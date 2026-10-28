namespace ProfilesApi.Application.DTOs.Requests.Registration;

public sealed record IdentityRegistrationRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role
);
