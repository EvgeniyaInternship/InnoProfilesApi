using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IDoctorService
{
    Task<DoctorDto> GetDoctorByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<DoctorDto>> GetDoctorsAsync(DoctorFilterDto? filter = null, CancellationToken ct = default);
    Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto, CancellationToken ct = default);
    Task UpdateDoctorAsync(UpdateDoctorDto dto, CancellationToken ct = default);
    Task RemoveDoctorAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<DoctorDto>> CreateRangeAsync(IEnumerable<CreateDoctorDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<UpdateDoctorDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
