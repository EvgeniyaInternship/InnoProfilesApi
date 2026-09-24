namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdatePatientDto(
    Guid Id,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);