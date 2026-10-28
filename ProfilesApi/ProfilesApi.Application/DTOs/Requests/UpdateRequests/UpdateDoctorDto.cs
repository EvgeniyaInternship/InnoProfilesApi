namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdateDoctorDto(
    Guid Id,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId,
    Guid SpecializationId
);
