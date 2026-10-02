namespace ProfilesApi.Application.DTOs.Filters;

public record ReceptionistFilterDto(
    Guid? OfficeId,
    DateTime? WorkStartDate,
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? SearchTerm
);