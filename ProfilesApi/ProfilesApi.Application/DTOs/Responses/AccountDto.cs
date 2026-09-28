using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Responses;

public sealed record AccountDto(
    Guid Id,
    string PhoneNumber,
    string Email,
    AccountRole Role,
    Guid? PhotoId,
    DateTime CreatedAt,
    DateTime UpdatedAt
);