using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdateAccountDto(
    Guid Id,
    string PhoneNumber,
    string Email,
    AccountRole Role,
    Guid? PhotoId
);
