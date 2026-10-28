namespace ProfilesApi.Application.DTOs.Filters;

public sealed record ReceptionistFilterDto(
    Guid? OfficeId,
    DateTime? WorkStartDate
);
