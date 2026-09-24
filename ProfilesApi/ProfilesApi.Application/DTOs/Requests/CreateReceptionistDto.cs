namespace ProfilesApi.Application.DTOs.Requests;

public record CreateReceptionistDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId
);