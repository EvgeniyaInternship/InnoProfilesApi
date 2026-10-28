namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreatePatientDto(
    Guid AccountId,
    DateTime BirthDate,
    string InsuranceNumber
);
