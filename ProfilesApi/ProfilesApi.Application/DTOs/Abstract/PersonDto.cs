namespace ProfilesApi.Application.DTOs.Abstract;

public abstract record PersonDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsDeleted,
    Guid AccountId,
    DateTime BirthDate,
    string FirstName,
    string LastName,
    string? MiddleName
) : SoftDeletableDto(Id, CreatedAt, UpdatedAt, IsDeleted);