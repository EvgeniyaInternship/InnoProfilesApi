using ProfilesApi.Domain.Enum;

namespace ProfilesApi.Application.DTOs.Filters;

public sealed record AccountFilterDto(
    string? Email,
    string? PhoneNumber,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? SearchTerm,
    AccountRole? Role
);
