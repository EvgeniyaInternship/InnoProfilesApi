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

public sealed class AdminService(IUnitOfWork unitOfWork) : IAdminService
{
    private static Expression<Func<AdminEntity, bool>> BuildFilterExpression(AdminFilterDto? filter)
    {
        if (filter is null)
            return x => true;

        return x => (filter.OfficeId == null || x.OfficeId == filter.OfficeId)
                 && (filter.WorkStartDateFrom == null || x.WorkStartDate >= filter.WorkStartDateFrom);
    }

    public async Task<AdminDto> GetAdminByIdAsync(Guid id, CancellationToken ct = default)
    {
        var admin = await unitOfWork.Admins.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (admin is null)
            throw new KeyNotFoundException($"Admin with ID {id} was not found.");

        return admin.ToDto();
    }

    public async Task<PagedResultDto<AdminDto>> GetAdminsAsync(
        PaginationParamsDto paginationParams,
        AdminFilterDto? filter = null,
        CancellationToken ct = default)
    {
        var filterExpression = BuildFilterExpression(filter);

        var admins = await unitOfWork.Admins.GetAllAsync(
            filterExpression, 
            paginationParams.PageNumber, 
            paginationParams.PageSize, 
            cancellationToken: ct);
        var totalCount = await unitOfWork.Admins.CountAsync(filterExpression, cancellationToken: ct);
        var totalPages = PaginationHelper.CalculateTotalPages(totalCount, paginationParams.PageSize);

        var dtos = admins.Select(admin => admin.ToDto()).ToList();

        return new PagedResultDto<AdminDto>(dtos, totalCount, totalPages, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AdminDto> CreateAdminAsync(CreateAdminDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == dto.AccountId, cancellationToken: ct);
            if (account is null)
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} was not found.");

            var existingAccountAdmin = await unitOfWork.Admins.GetAsync(x => x.AccountId == dto.AccountId, cancellationToken: ct);
            if (existingAccountAdmin is not null)
                throw new InvalidOperationException($"Admin with AccountId {dto.AccountId} already exists.");

            if (account.Role != AccountRole.Admin)
                throw new InvalidOperationException($"Account with ID {dto.AccountId} is not an admin.");

            var admin = dto.ToEntity();

            unitOfWork.Admins.Add(admin);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return admin.ToDto();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating admin.");
        }
    }

    public async Task UpdateAdminAsync(UpdateAdminDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var admin = await unitOfWork.Admins.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (admin is null)
                throw new KeyNotFoundException($"Admin with ID {dto.Id} was not found.");

            unitOfWork.Admins.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while updating admin.");
        }
    }

    public async Task RemoveAdminAsync(Guid id, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var admin = await unitOfWork.Admins.GetAsync(x => x.Id == id, true, cancellationToken: ct);
            if (admin is null)
                throw new KeyNotFoundException($"Admin with ID {id} was not found.");

            unitOfWork.Admins.Remove(admin);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while removing admin.");
        }
    }

    public async Task<List<AdminDto>> CreateRangeAsync(List<CreateAdminDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var accountIds = dtos.Select(x => x.AccountId);
            var existingAdmins = await unitOfWork.Admins.GetAllAsync(x => accountIds.Contains(x.AccountId), cancellationToken: ct);

            if (existingAdmins.Any())
            {
                var existingIds = existingAdmins.Select(a => a.AccountId);
                throw new InvalidOperationException($"Admins with AccountIds already exist: {string.Join(", ", existingIds)}");
            }

            var entities = dtos.Select(dto => dto.ToEntity());

            unitOfWork.Admins.AddRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entities.Select(admin => admin.ToDto()).ToList();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating admins.");
        }
    }

    public async Task UpdateRangeAsync(List<UpdateAdminDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id);
        if (uniqueDtos.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id);

            var existingAdmins = await unitOfWork.Admins.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);

            if (existingAdmins.Count != uniqueDtos.Count())
                throw new KeyNotFoundException("One or more admins were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Admins.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while updating admins.");
        }
    }

    public async Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct();
        if (distinctIds.Count() == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingAdmins = await unitOfWork.Admins.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct);

            if (existingAdmins.Count != distinctIds.Count())
                throw new KeyNotFoundException("One or more admins were not found.");

            unitOfWork.Admins.RemoveRange(existingAdmins);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while removing admins.");
        }
    }
}