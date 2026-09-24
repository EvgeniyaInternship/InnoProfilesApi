namespace ProfilesApi.Application.DTOs.Requests.Update;

public sealed record UpdateReceptionistDto(
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
);