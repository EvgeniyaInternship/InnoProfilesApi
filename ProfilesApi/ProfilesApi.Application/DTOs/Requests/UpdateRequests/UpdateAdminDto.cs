namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdateAdminDto(
    Guid Id,
    DateTime BirthDate,
    DateTime WorkStartDate,
    Guid OfficeId
);
