namespace ProfilesApi.Application.DTOs.Filters;

public sealed record DoctorFilterDto(
    Guid? OfficeId = null,
    Guid? SpecializationId = null,
    DateTime? WorkStartDate = null,
    string? FirstName = null,
    string? LastName = null,
    string? MiddleName = null,
    string? SearchTerm = null
);