namespace ProfilesApi.Application.DTOs.Requests.Update;

public record UpdateAdminDto(
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
);