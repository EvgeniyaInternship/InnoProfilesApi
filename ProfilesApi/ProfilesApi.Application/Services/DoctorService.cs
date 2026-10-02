using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;
using System.Linq.Expressions;

namespace ProfilesApi.Application.Services;

public class DoctorService(IUnitOfWork unitOfWork) : IDoctorService
{
    private static Expression<Func<DoctorEntity, bool>> BuildFilterExpression(DoctorFilterDto? filter)
    {
        if (filter is null)
            return x => true;

        var searchTerm = filter.SearchTerm?.Trim();

        return x => (filter.OfficeId == null || x.OfficeId == filter.OfficeId)
                 && (filter.SpecializationId == null || x.SpecializationId == filter.SpecializationId)
                 && (filter.WorkStartDate == null || x.WorkStartDate == filter.WorkStartDate)
                 && (string.IsNullOrWhiteSpace(filter.FirstName) || x.FirstName.Contains(filter.FirstName))
                 && (string.IsNullOrWhiteSpace(filter.LastName) || x.LastName.Contains(filter.LastName))
                 && (string.IsNullOrWhiteSpace(filter.MiddleName) || (x.MiddleName != null && x.MiddleName.Contains(filter.MiddleName)))
                 && (string.IsNullOrWhiteSpace(searchTerm) ||
                     x.FirstName.Contains(searchTerm) ||
                     x.LastName.Contains(searchTerm) ||
                     (x.MiddleName != null && x.MiddleName.Contains(searchTerm)));
    }

    public async Task<DoctorDto> GetDoctorByIdAsync(Guid id, CancellationToken ct = default)
    {
        var doctor = await unitOfWork.Doctors.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (doctor is null)
            throw new KeyNotFoundException($"Doctor with ID {id} was not found.");

        return doctor.ToDto();
    }

    public async Task<IEnumerable<DoctorDto>> GetDoctorsAsync(DoctorFilterDto? filter = null, CancellationToken ct = default)
    {
        var filterExpression = BuildFilterExpression(filter);

        var doctors = await unitOfWork.Doctors.GetAllAsync(filterExpression, cancellationToken: ct);

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
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var doctor = await unitOfWork.Doctors.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (doctor is null)
                throw new KeyNotFoundException($"Doctor with ID {dto.Id} was not found.");

            unitOfWork.Doctors.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while updating doctors.");
        }
    }

    public async Task RemoveDoctorAsync(Guid id, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var doctor = await unitOfWork.Doctors.GetAsync(x => x.Id == id, true, cancellationToken: ct);
            if (doctor is null)
                throw new KeyNotFoundException($"Doctor with ID {id} was not found.");


            unitOfWork.Doctors.Remove(doctor);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while removing doctors.");
        }
    }

    public async Task<IEnumerable<DoctorDto>> CreateRangeAsync(IEnumerable<CreateDoctorDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        var entities = dtos.Select(dto => dto.ToEntity());

        unitOfWork.Doctors.AddRange(entities);
        await unitOfWork.SaveChangesAsync(ct);

        return entities.Select(doctor => doctor.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateDoctorDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id).ToList();
        if (uniqueDtos.Count == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id).ToList();

            var existingDoctors = (await unitOfWork.Doctors.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct)).ToList();
            if (existingDoctors.Count != uniqueDtos.Count)
                throw new KeyNotFoundException("One or more doctors were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Doctors.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while updating doctors.");
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct().ToList();
        if (distinctIds.Count == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingDoctors = (await unitOfWork.Doctors.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct)).ToList();

            if (existingDoctors.Count != distinctIds.Count)
                throw new KeyNotFoundException("One or more doctors were not found.");

            unitOfWork.Doctors.RemoveRange(existingDoctors);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while removing doctors.");
        }
    }
}