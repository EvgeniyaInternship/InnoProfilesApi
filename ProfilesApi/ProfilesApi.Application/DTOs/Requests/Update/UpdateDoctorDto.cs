namespace ProfilesApi.Application.DTOs.Requests.Update;

public record UpdateDoctorDto(
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId,
    Guid SpecializationId
);