using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;

namespace ProfilesApi.Application.Services;

public class DoctorService(IUnitOfWork unitOfWork) : IDoctorService
{
    public async Task<DoctorDto> GetDoctorByIdAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await unitOfWork.Doctors.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (doctor is null)
            throw new KeyNotFoundException($"Doctor with ID {id} was not found.");

        return doctor.ToDto();
    }

    public async Task<IEnumerable<DoctorDto>> GetAllDoctorsByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var doctors = await unitOfWork.Doctors.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);
        return doctors.Select(doctor => doctor.ToDto());
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto, CancellationToken ct = default)
    {
        var doctor = dto.ToEntity();

        unitOfWork.Doctors.Add(doctor);
        await unitOfWork.SaveChangesAsync(ct);

        return doctor.ToDto();
    }

    public async Task UpdateDoctorAsync(UpdateDoctorDto dto, CancellationToken ct = default)
    {
        var doctor = await unitOfWork.Doctors.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
        if (doctor is null)
            throw new KeyNotFoundException($"Doctor with ID {dto.Id} was not found.");

        unitOfWork.Doctors.Update(dto.UpdateEntity());
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveDoctorAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await unitOfWork.Doctors.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (doctor is null)
            throw new KeyNotFoundException($"Doctor with ID {id} was not found.");

        unitOfWork.Doctors.Remove(doctor);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<DoctorDto>> CreateRangeAsync(IEnumerable<CreateDoctorDto> dtos, CancellationToken ct = default)
    {
        var dtoList = dtos.ToList();
        if (dtoList.Count == 0) return [];

        var entities = dtoList.Select(dto => dto.ToEntity()).ToList();

        unitOfWork.Doctors.AddRange(entities);
        await unitOfWork.SaveChangesAsync(ct);

        return entities.Select(doctor => doctor.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateDoctorDto> dtos, CancellationToken ct = default)
    {
        var entities = dtos.Select(d => d.UpdateEntity()).Distinct().ToList();
        if (entities.Count == 0) return;

        unitOfWork.Doctors.UpdateRange(entities);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var entitiesToDelete = ids.Distinct().Select(id => new DoctorEntity { Id = id }).ToList();

        if (entitiesToDelete.Count == 0) return;

        unitOfWork.Doctors.RemoveRange(entitiesToDelete);
        await unitOfWork.SaveChangesAsync(ct);
    }
}