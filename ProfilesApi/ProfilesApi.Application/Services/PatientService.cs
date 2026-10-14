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

public sealed class PatientService(IUnitOfWork unitOfWork) : IPatientService
{
    private static Expression<Func<PatientEntity, bool>> BuildFilterExpression(PatientFilterDto? filter)
    {
        if (filter is null)
            return x => true;

        var searchTerm = filter.SearchTerm?.Trim();

        return x => (string.IsNullOrWhiteSpace(filter.InsuranceNumber) || x.InsuranceNumber.Contains(filter.InsuranceNumber))
                 && (string.IsNullOrWhiteSpace(filter.FirstName) || x.FirstName.Contains(filter.FirstName))
                 && (string.IsNullOrWhiteSpace(filter.LastName) || x.LastName.Contains(filter.LastName))
                 && (string.IsNullOrWhiteSpace(filter.MiddleName) || (x.MiddleName != null && x.MiddleName.Contains(filter.MiddleName)))
                 && (string.IsNullOrWhiteSpace(searchTerm) ||
                     x.FirstName.Contains(searchTerm) ||
                     x.LastName.Contains(searchTerm) ||
                     (x.MiddleName != null && x.MiddleName.Contains(searchTerm)) ||
                     x.InsuranceNumber.Contains(searchTerm));
    }

    public async Task<PatientDto> GetPatientByIdAsync(Guid id, CancellationToken ct = default)
    {
        var patient = await unitOfWork.Patients.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (patient is null)
            throw new KeyNotFoundException($"Patient with ID {id} was not found.");

        return patient.ToDto();
    }

    public async Task<PagedResultDto<PatientDto>> GetPatientsAsync(
        PaginationParamsDto paginationParams,
        PatientFilterDto? filter = null,
        CancellationToken ct = default)
    {
        var filterExpression = BuildFilterExpression(filter);

        var patients = await unitOfWork.Patients.GetAllAsync(
            filterExpression,
            paginationParams.PageNumber,
            paginationParams.PageSize,
            cancellationToken: ct);
        var totalCount = await unitOfWork.Patients.CountAsync(filterExpression, cancellationToken: ct);
        var totalPages = PaginationHelper.CalculateTotalPages(totalCount, paginationParams.PageSize);

        var dtos = patients.Select(patient => patient.ToDto()).ToList();

        return new PagedResultDto<PatientDto>(dtos, totalCount, totalPages, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<PatientDto> CreatePatientAsync(CreatePatientDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == dto.AccountId, cancellationToken: ct);
            if (account is null)
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} was not found.");

            var existingAccountPatient = await unitOfWork.Patients.GetAsync(x => x.AccountId == dto.AccountId, cancellationToken: ct);
            if (existingAccountPatient is not null)
                throw new InvalidOperationException($"Patient with AccountId {dto.AccountId} already exists.");

            if (account.Role != AccountRole.Patient)
                throw new InvalidOperationException($"Account with ID {dto.AccountId} is not a patient.");

            var patient = dto.ToEntity();

            unitOfWork.Patients.Add(patient);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return patient.ToDto();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating patient.");
        }
    }

    public async Task UpdatePatientAsync(UpdatePatientDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var patient = await unitOfWork.Patients.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (patient is null)
                throw new KeyNotFoundException($"Patient with ID {dto.Id} was not found.");

            unitOfWork.Patients.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while updating patient.");
        }
    }

    public async Task RemovePatientAsync(Guid id, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var patient = await unitOfWork.Patients.GetAsync(x => x.Id == id, true, cancellationToken: ct);
            if (patient is null)
                throw new KeyNotFoundException($"Patient with ID {id} was not found.");
            unitOfWork.Patients.Remove(patient);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while removing patients.");
        }
    }

    public async Task<List<PatientDto>> CreateRangeAsync(List<CreatePatientDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var accountIds = dtos.Select(x => x.AccountId);
            var existingPatients = await unitOfWork.Patients.GetAllAsync(
                x => accountIds.Contains(x.AccountId),
                cancellationToken: ct);

            if (existingPatients.Any())
            {
                var existingIds = existingPatients.Select(a => a.AccountId);
                throw new InvalidOperationException($"Patients with AccountIds already exist: {string.Join(", ", existingIds)}");
            }

            var entities = dtos.Select(dto => dto.ToEntity());

            unitOfWork.Patients.AddRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entities.Select(patient => patient.ToDto()).ToList();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating patients.");
        }
    }

    public async Task UpdateRangeAsync(List<UpdatePatientDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id);
        if (uniqueDtos.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id);

            var existingPatients = await unitOfWork.Patients.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);

            if (existingPatients.Count != uniqueDtos.Count())
                throw new KeyNotFoundException("One or more patients were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Patients.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while updating patients.");
        }
    }

    public async Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct();
        if (distinctIds.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingPatients = await unitOfWork.Patients.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct);

            if (existingPatients.Count != distinctIds.Count())
                throw new KeyNotFoundException("One or more patients were not found.");

            unitOfWork.Patients.RemoveRange(existingPatients);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while removing patients.");
        }
    }
}
