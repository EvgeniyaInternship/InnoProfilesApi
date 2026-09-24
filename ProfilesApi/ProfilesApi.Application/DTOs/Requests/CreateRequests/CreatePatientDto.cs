namespace ProfilesApi.Application.DTOs.Requests.CreateRequests;

public sealed record CreatePatientDto(
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    string InsuranceNumber
);