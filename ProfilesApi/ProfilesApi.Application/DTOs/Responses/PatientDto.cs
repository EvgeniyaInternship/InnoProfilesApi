using ProfilesApi.Application.DTOs.Abstract;

namespace ProfilesApi.Application.DTOs.Responses;

public record PatientDto(
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
) : PersonDto(Id, CreatedAt, UpdatedAt, IsDeleted, AccountId, BirthDate, FirstName, LastName, MiddleName);