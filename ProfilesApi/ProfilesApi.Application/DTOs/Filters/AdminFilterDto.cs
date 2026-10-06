namespace ProfilesApi.Application.DTOs.Filters;

public sealed record AdminFilterDto(
    Guid? OfficeId,
    DateTime? WorkStartDateFrom,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? SearchTerm
    );