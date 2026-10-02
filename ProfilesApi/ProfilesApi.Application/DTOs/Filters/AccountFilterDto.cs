using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Filters;

public sealed record AccountFilterDto(
    string? Email,
    string? PhoneNumber,
    string? SearchTerm,
    AccountRole? Role
);
