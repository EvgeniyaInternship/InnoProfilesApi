namespace ProfilesApi.Application.DTOs.Requests.Update;

public sealed record UpdatePatientDto(
    Guid Id,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);