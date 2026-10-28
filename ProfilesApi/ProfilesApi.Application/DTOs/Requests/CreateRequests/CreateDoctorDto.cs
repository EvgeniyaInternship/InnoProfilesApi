namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreateDoctorDto(
    Guid AccountId,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId,
    Guid SpecializationId
);
