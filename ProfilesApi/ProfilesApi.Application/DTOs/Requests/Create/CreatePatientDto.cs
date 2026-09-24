namespace ProfilesApi.Application.DTOs.Requests.Create;

public sealed record CreatePatientDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);