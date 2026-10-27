namespace ProfilesApi.Application.DTOs.Filters;

public sealed record PatientFilterDto(
    string? InsuranceNumber,
    string? SearchTerm
);
