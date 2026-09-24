namespace ProfilesApi.Application.DTOs.Requests.Update;

public sealed record UpdateDoctorDto(
    Guid Id,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId,
    Guid SpecializationId
);