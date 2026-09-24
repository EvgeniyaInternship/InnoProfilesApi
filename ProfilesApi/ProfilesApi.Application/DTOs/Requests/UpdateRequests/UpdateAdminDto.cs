namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdateAdminDto(
    Guid Id,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
);