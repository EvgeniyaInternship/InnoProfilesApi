using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IReceptionistService
{
    Task<ReceptionistDto> GetReceptionistByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<ReceptionistDto>> GetReceptionistsAsync(
        PaginationParamsDto paginationParams,
        ReceptionistFilterDto? filter = null,
        CancellationToken ct = default);
    Task<ReceptionistDto> CreateReceptionistAsync(CreateReceptionistDto dto, CancellationToken ct = default);
    Task UpdateReceptionistAsync(UpdateReceptionistDto dto, CancellationToken ct = default);
    Task RemoveReceptionistAsync(Guid id, CancellationToken ct = default);
    Task<List<ReceptionistDto>> CreateRangeAsync(List<CreateReceptionistDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(List<UpdateReceptionistDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default);
}
