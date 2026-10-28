namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreateAdminDto(
    Guid AccountId,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId
);
