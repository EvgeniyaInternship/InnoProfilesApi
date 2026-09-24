namespace ProfilesApi.Application.DTOs.Responses;

public sealed record DoctorDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsDeleted,
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime WorkStartDate,
    Guid OfficeId,
    Guid SpecializationId
);
