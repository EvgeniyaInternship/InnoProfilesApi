namespace ProfilesApi.Application.DTOs.Requests.Update;

public sealed record UpdatePatientDto(
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);