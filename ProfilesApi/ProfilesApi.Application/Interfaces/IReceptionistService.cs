using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IReceptionistService
{
    Task<ReceptionistDto> GetReceptionistByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ReceptionistDto>> GetAllReceptionistsByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
    Task<ReceptionistDto> CreateReceptionistAsync(CreateReceptionistDto dto, CancellationToken ct = default);
    Task UpdateReceptionistAsync(UpdateReceptionistDto dto, CancellationToken ct = default);
    Task RemoveReceptionistAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ReceptionistDto>> CreateRangeAsync(IEnumerable<CreateReceptionistDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<UpdateReceptionistDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
