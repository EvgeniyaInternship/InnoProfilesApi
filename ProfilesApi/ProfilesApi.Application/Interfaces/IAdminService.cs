using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IAdminService
{
    Task<AdminDto> GetAdminByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<AdminDto>> GetAdminsAsync(
        AdminFilterDto? filter,
        PaginationParams paginationParams,
        CancellationToken ct = default);
    Task<AdminDto> CreateAdminAsync(CreateAdminDto dto, CancellationToken ct = default);
    Task UpdateAdminAsync(UpdateAdminDto dto, CancellationToken ct = default);
    Task RemoveAdminAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<AdminDto>> CreateRangeAsync(IEnumerable<CreateAdminDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<UpdateAdminDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
