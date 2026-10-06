using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IPatientService
{
    Task<PatientDto> GetPatientByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<PatientDto>> GetPatientsAsync(
        PatientFilterDto? filter,
        PaginationParams paginationParams,
        CancellationToken ct = default);
    Task<PatientDto> CreatePatientAsync(CreatePatientDto dto, CancellationToken ct = default);
    Task UpdatePatientAsync(UpdatePatientDto dto, CancellationToken ct = default);
    Task RemovePatientAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<PatientDto>> CreateRangeAsync(IEnumerable<CreatePatientDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<UpdatePatientDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}