namespace ProfilesApi.Application.DTOs.Filters;

public sealed record AdminFilterDto(
    Guid? OfficeId,
    DateTime? WorkStartDateFrom
);
