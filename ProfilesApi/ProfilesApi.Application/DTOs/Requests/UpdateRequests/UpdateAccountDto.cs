using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public record UpdateAccountDto(
    Guid Id,
    string PhoneNumber,
    string Email,
    AccountRole Role,
    Guid? PhotoId
);
