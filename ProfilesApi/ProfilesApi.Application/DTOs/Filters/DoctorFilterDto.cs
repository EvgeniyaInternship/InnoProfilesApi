namespace ProfilesApi.Application.DTOs.Filters;

public sealed record DoctorFilterDto(
    Guid? OfficeId = null,
    Guid? SpecializationId = null,
    DateTime? WorkStartDate = null
);
