namespace ProfilesApi.Application.DTOs.Common;

public sealed record PaginationParamsDto(
    int PageNumber,
    int PageSize
);
