namespace ProfilesApi.Application.DTOs.Requests.Create;

public sealed record CreateAdminDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
);