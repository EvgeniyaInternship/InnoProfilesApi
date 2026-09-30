using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class ReceptionistService(IUnitOfWork unitOfWork) : IReceptionistService
{
    public async Task<ReceptionistDto> GetReceptionistByIdAsync(Guid id, CancellationToken ct = default)
    {
        var receptionist = await unitOfWork.Receptionists.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (receptionist is null)
            throw new KeyNotFoundException($"Receptionist with ID {id} was not found.");

        return receptionist.ToDto();
    }

    public async Task<IEnumerable<ReceptionistDto>> GetAllReceptionistsByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var receptionists = await unitOfWork.Receptionists.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);
        return receptionists.Select(receptionist => receptionist.ToDto());
    }

    public async Task<ReceptionistDto> CreateReceptionistAsync(CreateReceptionistDto dto, CancellationToken ct = default)
    {
        var receptionist = dto.ToEntity();

        unitOfWork.Receptionists.Add(receptionist);
        await unitOfWork.SaveChangesAsync(ct);

        return receptionist.ToDto();
    }

    public async Task UpdateReceptionistAsync(UpdateReceptionistDto dto, CancellationToken ct = default)
    {
        var receptionist = await unitOfWork.Receptionists.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
        if (receptionist is null)
            throw new KeyNotFoundException($"Receptionist with ID {dto.Id} was not found.");

        unitOfWork.Receptionists.Update(dto.UpdateEntity());
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveReceptionistAsync(Guid id, CancellationToken ct = default)
    {
        var receptionist = await unitOfWork.Receptionists.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (receptionist is null)
            throw new KeyNotFoundException($"Receptionist with ID {id} was not found.");

        unitOfWork.Receptionists.Remove(receptionist);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<ReceptionistDto>> CreateRangeAsync(IEnumerable<CreateReceptionistDto> dtos, CancellationToken ct = default)
    {
        var dtoList = dtos.ToList();
        if (dtoList.Count == 0) return [];

        var entities = dtoList.Select(dto => dto.ToEntity()).ToList();

        unitOfWork.Receptionists.AddRange(entities);
        await unitOfWork.SaveChangesAsync(ct);

        return entities.Select(receptionist => receptionist.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateReceptionistDto> dtos, CancellationToken ct = default)
    {
        var entities = dtos.Select(d => d.UpdateEntity()).Distinct().ToList();
        if (entities.Count == 0) return;

        unitOfWork.Receptionists.UpdateRange(entities);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var entitiesToDelete = ids.Distinct().Select(id => new ReceptionistEntity { Id = id }).ToList();

        if (entitiesToDelete.Count == 0) return;

        unitOfWork.Receptionists.RemoveRange(entitiesToDelete);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
