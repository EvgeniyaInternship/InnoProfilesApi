namespace ProfilesApi.Application.DTOs.Abstract;

public abstract record SoftDeletableDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool IsDeleted
) : BaseDto(Id, CreatedAt, UpdatedAt);
