namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdateReceptionistDto(
    Guid Id,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId
);
