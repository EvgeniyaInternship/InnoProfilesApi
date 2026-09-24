using ProfilesApi.Application.DTOs.Abstract;

namespace ProfilesApi.Application.DTOs.Responses;

public record AdminDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsDeleted,
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
) : PersonDto(Id, CreatedAt, UpdatedAt, IsDeleted, AccountId, BirthDate, FirstName, LastName, MiddleName);