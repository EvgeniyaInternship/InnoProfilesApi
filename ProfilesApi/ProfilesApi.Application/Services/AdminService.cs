using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Application.Services;

public class AdminService(IUnitOfWork unitOfWork) : IAdminService
{
    public async Task<AdminDto> GetAdminByIdAsync(Guid id, CancellationToken ct = default)
    {
        var admin = await unitOfWork.Admins.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (admin is null)
            throw new KeyNotFoundException($"Admin with ID {id} was not found.");

        return admin.ToDto();
    }

    public async Task<IEnumerable<AdminDto>> GetAllAdminsByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var admins = await unitOfWork.Admins.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);
        return admins.Select(admin => admin.ToDto());
    }

    public async Task<AdminDto> CreateAdminAsync(CreateAdminDto dto, CancellationToken ct = default)
    {
        var admin = dto.ToEntity();

        unitOfWork.Admins.Add(admin);
        await unitOfWork.SaveChangesAsync(ct);

        return admin.ToDto();
    }

    public async Task UpdateAdminAsync(UpdateAdminDto dto, CancellationToken ct = default)
    {
        var admin = await unitOfWork.Admins.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
        if (admin is null)
            throw new KeyNotFoundException($"Admin with ID {dto.Id} was not found.");

        unitOfWork.Admins.Update(dto.UpdateEntity());
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveAdminAsync(Guid id, CancellationToken ct = default)
    {
        var admin = await unitOfWork.Admins.GetAsync(x => x.Id == id, true, cancellationToken: ct);
        if (admin is null)
            throw new KeyNotFoundException($"Admin with ID {id} was not found.");

        unitOfWork.Admins.Remove(admin);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<AdminDto>> CreateRangeAsync(IEnumerable<CreateAdminDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        var entities = dtos.Select(dto => dto.ToEntity()).ToList();

        unitOfWork.Admins.AddRange(entities);
        await unitOfWork.SaveChangesAsync(ct);

        return entities.Select(admin => admin.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateAdminDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return;
        var uniqueDtos = dtos.DistinctBy(d => d.Id).ToList();
        var entities = uniqueDtos.Select(d => d.UpdateEntity()).ToList();

        unitOfWork.Admins.UpdateRange(entities);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var entitiesToDelete = ids.Distinct().Select(id => new AdminEntity { Id = id }).ToList();

        if (entitiesToDelete.Count == 0) return;

        unitOfWork.Admins.RemoveRange(entitiesToDelete);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
