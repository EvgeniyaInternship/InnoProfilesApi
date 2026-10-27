namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreateReceptionistDto(
    Guid AccountId,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId
);
