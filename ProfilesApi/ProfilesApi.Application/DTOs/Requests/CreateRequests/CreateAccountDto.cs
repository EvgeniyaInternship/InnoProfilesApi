using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public record CreateAccountDto(
    string PhoneNumber,
    string Email,
    string PasswordHash,
    AccountRole Role,
    Guid? PhotoId
);