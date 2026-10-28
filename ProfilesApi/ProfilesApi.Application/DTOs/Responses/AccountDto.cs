using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Responses;

public sealed record AccountDto(
    Guid Id,
    string PhoneNumber,
    string Email,
    AccountRole Role,
    Guid? PhotoId,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
