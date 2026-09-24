namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreateDoctorDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId,
    Guid SpecializationId
);