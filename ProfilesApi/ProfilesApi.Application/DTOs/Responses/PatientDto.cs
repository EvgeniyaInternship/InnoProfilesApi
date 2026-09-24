namespace ProfilesApi.Application.DTOs.Responses;

public sealed record PatientDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsDeleted,
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);