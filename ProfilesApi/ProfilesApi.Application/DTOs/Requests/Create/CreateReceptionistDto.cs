namespace ProfilesApi.Application.DTOs.Requests.Create;

public sealed record UpdateReceptionistDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
);