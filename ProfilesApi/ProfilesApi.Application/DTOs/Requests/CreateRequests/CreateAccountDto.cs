using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreateAccountDto(
    string PhoneNumber,
    string Email,
    string PasswordHash,
    AccountRole Role,
    Guid? PhotoId,
    string FirstName,
    string LastName,
    string? MiddleName
);
