using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Helpers;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Enum;
using ProfilesApi.Domain.Exceptions;
using ProfilesApi.Domain.Interfaces;
using System.Linq.Expressions;

namespace ProfilesApi.Application.Services;

public sealed class DoctorService(IUnitOfWork unitOfWork) : IDoctorService
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

    public async Task<PagedResultDto<DoctorDto>> GetDoctorsAsync(
        PaginationParamsDto paginationParams,
        DoctorFilterDto? filter = null,
        CancellationToken ct = default)
    {
        var filterExpression = BuildFilterExpression(filter);

        var doctors = await unitOfWork.Doctors.GetAllAsync(
            filterExpression, 
            paginationParams.PageNumber, 
            paginationParams.PageSize, 
            cancellationToken: ct);
        var totalCount = await unitOfWork.Doctors.CountAsync(filterExpression, cancellationToken: ct);
        var totalPages = PaginationHelper.CalculateTotalPages(totalCount, paginationParams.PageSize);

        var dtos = doctors.Select(doctor => doctor.ToDto()).ToList();

        return new PagedResultDto<DoctorDto>(dtos, totalCount, totalPages, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<DoctorDto> CreateDoctorAsync(CreateDoctorDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == dto.AccountId, cancellationToken: ct);
            if (account is null)
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} was not found.");

            var existingAccountDoctor = await unitOfWork.Doctors.GetAsync(x => x.AccountId == dto.AccountId, cancellationToken: ct);
            if (existingAccountDoctor is not null)
                throw new InvalidOperationException($"Doctor with AccountId {dto.AccountId} already exists.");

            if (account.Role != AccountRole.Doctor)
                throw new InvalidOperationException($"Account with ID {dto.AccountId} is not a doctor.");

            var doctor = dto.ToEntity();

            unitOfWork.Doctors.Add(doctor);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return doctor.ToDto();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating doctor.");
        }
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
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while updating doctor.");
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
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while removing doctor.");
        }
    }

    public async Task<List<DoctorDto>> CreateRangeAsync(List<CreateDoctorDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var accountIds = dtos.Select(x => x.AccountId);
            var existingDoctors = await unitOfWork.Doctors.GetAllAsync(
                x => accountIds.Contains(x.AccountId),
                cancellationToken: ct);

            if (existingDoctors.Any())
            {
                var existingIds = existingDoctors.Select(a => a.AccountId);
                throw new InvalidOperationException($"Doctors with AccountIds already exist: {string.Join(", ", existingIds)}");
            }

            var entities = dtos.Select(dto => dto.ToEntity());

            unitOfWork.Doctors.AddRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entities.Select(doctor => doctor.ToDto()).ToList();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating doctors.");
        }
    }

    public async Task UpdateRangeAsync(List<UpdateDoctorDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id);
        if (uniqueDtos.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id);

            var existingDoctors = await unitOfWork.Doctors.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);

            if (existingDoctors.Count != uniqueDtos.Count())
                throw new KeyNotFoundException("One or more doctors were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Doctors.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while updating doctors.");
        }
    }

    public async Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct();
        if (distinctIds.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingDoctors = await unitOfWork.Doctors.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct);

            if (existingDoctors.Count != distinctIds.Count())
                throw new KeyNotFoundException("One or more doctors were not found.");

            unitOfWork.Doctors.RemoveRange(existingDoctors);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while removing doctors.");
        }
    }
}