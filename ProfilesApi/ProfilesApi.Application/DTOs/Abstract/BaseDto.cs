namespace ProfilesApi.Application.DTOs.Abstract;

public abstract record BaseDto(
    Guid Id,
    DateTime CreatedAt,
    DateTime UpdatedAt
);