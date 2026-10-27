namespace ProfilesApi.Application.DTOs.Requests.UpdateRequests;

public sealed record UpdatePatientDto(
    Guid Id,
    DateTime BirthDate,
    string InsuranceNumber
);
