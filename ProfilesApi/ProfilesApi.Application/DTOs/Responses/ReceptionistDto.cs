namespace ProfilesApi.Application.DTOs.Responses;

public sealed record ReceptionistDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    Guid AccountId,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId
);
