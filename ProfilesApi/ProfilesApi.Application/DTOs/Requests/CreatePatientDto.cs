namespace ProfilesApi.Application.DTOs.Requests;

public record CreatePatientDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);