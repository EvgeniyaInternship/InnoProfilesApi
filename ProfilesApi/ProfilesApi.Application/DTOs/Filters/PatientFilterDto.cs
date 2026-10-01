namespace ProfilesApi.Application.DTOs.Filters;

public record PatientFilterDto(
    string? InsuranceNumber,
    string? FirstName,
    string? LastName,
    string? MiddleName, 
    string? SearchTerm
);
