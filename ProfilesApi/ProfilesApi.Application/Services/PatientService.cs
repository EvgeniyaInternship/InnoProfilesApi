using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Infrastructure.Interfaces;

namespace ProfilesApi.Application.Services;

public class PatientService(IUnitOfWork unitOfWork) : IPatientService
{
    public async Task<PatientDto> GetPatientByIdAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await unitOfWork.Patients.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (patient is null)
            throw new KeyNotFoundException($"Patient with ID {id} was not found.");

        return patient.ToDto();
    }

    public async Task<IEnumerable<PatientDto>> GetAllPatientsAsync(CancellationToken ct = default)
    {
        var patients = await unitOfWork.Patients.GetAllAsync(cancellationToken: ct);
        return patients.Select(patient => patient.ToDto());
    }

    public async Task<PatientDto> CreatePatientAsync(CreatePatientDto dto, CancellationToken ct = default)
    {
        var patient = dto.ToEntity();

        unitOfWork.Patients.Add(patient);
        await unitOfWork.SaveChangesAsync(ct);

        return patient.ToDto();
    }

    public async Task UpdatePatientAsync(UpdatePatientDto dto, CancellationToken ct = default)
    {
        var patient = await unitOfWork.Patients.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
        if (patient is null)
            throw new KeyNotFoundException($"Patient with ID {dto.Id} was not found.");

        unitOfWork.Patients.Update(dto.UpdateEntity());
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemovePatientAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await unitOfWork.Patients.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (patient is null)
            throw new KeyNotFoundException($"Patient with ID {id} was not found.");

        unitOfWork.Patients.Remove(patient);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task<IEnumerable<PatientDto>> CreateRangeAsync(IEnumerable<CreatePatientDto> dtos, CancellationToken ct = default)
    {
        var dtoList = dtos.ToList();
        if (dtoList.Count == 0) return [];

        var entities = dtoList.Select(dto => dto.ToEntity()).ToList();

        unitOfWork.Patients.AddRange(entities);
        await unitOfWork.SaveChangesAsync(ct);

        return entities.Select(patient => patient.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdatePatientDto> dtos, CancellationToken ct = default)
    {
        var entities = dtos.Select(d => d.UpdateEntity()).Distinct().ToList();
        if (entities.Count == 0) return;

        unitOfWork.Patients.UpdateRange(entities);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var entitiesToDelete = ids.Distinct().Select(id => new PatientEntity { Id = id }).ToList();

        if (entitiesToDelete.Count == 0) return;

        unitOfWork.Patients.RemoveRange(entitiesToDelete);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
