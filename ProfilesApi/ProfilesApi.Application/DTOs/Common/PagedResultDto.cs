namespace ProfilesApi.Application.DTOs.Common;

public sealed record PagedResultDto<T>(
    IEnumerable<T> Items,
    int TotalPages,
    int PageNumber,
    int PageSize);