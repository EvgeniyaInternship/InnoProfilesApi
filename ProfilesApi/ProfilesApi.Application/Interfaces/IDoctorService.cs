using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IDoctorService
{
    Task<DoctorDto> GetDoctorByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
        PaginationParamsDto paginationParams,
        DoctorFilterDto? filter = null,
        CancellationToken ct = default);
    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto, CancellationToken ct = default);
    Task UpdateDoctorAsync(UpdateDoctorDto dto, CancellationToken ct = default);
    Task RemoveDoctorAsync(Guid id, CancellationToken ct = default);
    Task<List<DoctorDto>> CreateRangeAsync(List<CreateDoctorDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(List<UpdateDoctorDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default);
}
