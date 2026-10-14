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
using ProfilesApi.Domain.Interfaces;
using System.Linq.Expressions;

namespace ProfilesApi.Application.Services;

public sealed class ReceptionistService(IUnitOfWork unitOfWork) : IReceptionistService
{
    private static Expression<Func<ReceptionistEntity, bool>> BuildFilterExpression(ReceptionistFilterDto? filter)
    {
        if (filter is null)
            return x => true;
        
        var searchTerm = filter.SearchTerm?.Trim();

        return x => (filter.OfficeId == null || x.OfficeId == filter.OfficeId)
                 && (filter.WorkStartDate == null || x.WorkStartDate >= filter.WorkStartDate)
                 && (string.IsNullOrWhiteSpace(filter.FirstName) || x.FirstName.Contains(filter.FirstName))
                 && (string.IsNullOrWhiteSpace(filter.LastName) || x.LastName.Contains(filter.LastName))
                 && (string.IsNullOrWhiteSpace(filter.MiddleName) || (x.MiddleName != null && x.MiddleName.Contains(filter.MiddleName)))
                 && (string.IsNullOrWhiteSpace(searchTerm) ||
                     x.FirstName.Contains(searchTerm) ||
                     x.LastName.Contains(searchTerm) ||
                     (x.MiddleName != null && x.MiddleName.Contains(searchTerm)));
    }

    public async Task<ReceptionistDto> GetReceptionistByIdAsync(Guid id, CancellationToken ct = default)
    {
        var receptionist = await unitOfWork.Receptionists.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (receptionist is null)
            throw new KeyNotFoundException($"Receptionist with ID {id} was not found.");

        return receptionist.ToDto();
    }

    public async Task<PagedResultDto<ReceptionistDto>> GetReceptionistsAsync(
        PaginationParamsDto paginationParams,
        ReceptionistFilterDto? filter = null,
        CancellationToken ct = default)
    {
        var filterExpression = BuildFilterExpression(filter);

        var receptionists = await unitOfWork.Receptionists.GetAllAsync(
            filterExpression,
            paginationParams.PageNumber,
            paginationParams.PageSize,
            cancellationToken: ct);
        var totalCount = await unitOfWork.Receptionists.CountAsync(filterExpression, cancellationToken: ct);
        var totalPages = PaginationHelper.CalculateTotalPages(totalCount, paginationParams.PageSize);

        var dtos = receptionists.Select(receptionist => receptionist.ToDto()).ToList();

        return new PagedResultDto<ReceptionistDto>(dtos, totalCount, totalPages, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<ReceptionistDto> CreateReceptionistAsync(CreateReceptionistDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == dto.AccountId, cancellationToken: ct);
            if (account is null)
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} was not found.");

            var existingAccountReceptionist = await unitOfWork.Receptionists.GetAsync(x => x.AccountId == dto.AccountId, cancellationToken: ct);
            if (existingAccountReceptionist is not null)
                throw new InvalidOperationException($"Receptionist with AccountId {dto.AccountId} already exists.");

            if (account.Role != AccountRole.Receptionist)
                throw new InvalidOperationException($"Account with ID {dto.AccountId} is not a receptionist.");

            var receptionist = dto.ToEntity();

            unitOfWork.Receptionists.Add(receptionist);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return receptionist.ToDto();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while creating receptionist.");
        }
    }

    public async Task UpdateReceptionistAsync(UpdateReceptionistDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var receptionist = await unitOfWork.Receptionists.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (receptionist is null)
                throw new KeyNotFoundException($"Receptionist with ID {dto.Id} was not found.");

            unitOfWork.Receptionists.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while updating receptionist.");
        }
    }

    public async Task RemoveReceptionistAsync(Guid id, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var receptionist = await unitOfWork.Receptionists.GetAsync(x => x.Id == id, true, cancellationToken: ct);
            if (receptionist is null)
                throw new KeyNotFoundException($"Receptionist with ID {id} was not found.");

            unitOfWork.Receptionists.Remove(receptionist);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while removing receptionists.");
        }
    }

    public async Task<List<ReceptionistDto>> CreateRangeAsync(List<CreateReceptionistDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var accountIds = dtos.Select(x => x.AccountId);
            var existingReceptionists = await unitOfWork.Receptionists.GetAllAsync(
                x => accountIds.Contains(x.AccountId),
                cancellationToken: ct);

            if (existingReceptionists.Any())
            {
                var existingIds = existingReceptionists.Select(a => a.AccountId);
                throw new InvalidOperationException($"Receptionists with AccountIds already exist: {string.Join(", ", existingIds)}");
            }

            var entities = dtos.Select(dto => dto.ToEntity());

            unitOfWork.Receptionists.AddRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entities.Select(receptionist => receptionist.ToDto()).ToList();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while creating receptionists.");
        }
    }

    public async Task UpdateRangeAsync(List<UpdateReceptionistDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id);
        if (uniqueDtos.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id);

            var existingReceptionists = await unitOfWork.Receptionists.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);

            if (existingReceptionists.Count != uniqueDtos.Count())
                throw new KeyNotFoundException("One or more receptionists were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Receptionists.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while updating receptionists.");
        }
    }

    public async Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct();
        if (distinctIds.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingReceptionists = await unitOfWork.Receptionists.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct);

            if (existingReceptionists.Count != distinctIds.Count())
                throw new KeyNotFoundException("One or more receptionists were not found.");

            unitOfWork.Receptionists.RemoveRange(existingReceptionists);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw new Exception("An error occurred while removing receptionists.");
        }
    }
}
