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
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace ProfilesApi.Application.Services;

public sealed class AdminService(IUnitOfWork unitOfWork, ILogger<AdminService> logger) : IAdminService
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
        {
            logger.LogWarning("Admin with id {AdminId} does not exist.", id);
            throw new KeyNotFoundException($"Admin with ID {id} was not found.");
        }

        logger.LogInformation("Loaded admin {AdminId}.", admin.Id);
        return admin.ToDto();
    }

    public async Task<AdminDto> GetAdminByAccountIdAsync(Guid accountId, CancellationToken ct = default)
    {
        var admin = await unitOfWork.Admins.GetAsync(x => x.AccountId == accountId, cancellationToken: ct);
        if (admin is null)
        {
            logger.LogWarning("Admin for account {AccountId} does not exist.", accountId);
            throw new KeyNotFoundException($"Admin for account {accountId} was not found.");
        }

        logger.LogInformation("Loaded admin {AdminId} for account {AccountId}.", admin.Id, accountId);
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

        logger.LogInformation("Loaded {Count} admins.", dtos.Count);
        return new PagedResultDto<AdminDto>(dtos, totalCount, totalPages, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AdminDto> CreateAdminAsync(CreateAdminDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == dto.AccountId, cancellationToken: ct);
            if (account is null)
            {
                logger.LogWarning("Account with id {AccountId} does not exist.", dto.AccountId);
                throw new KeyNotFoundException($"Account with ID {dto.AccountId} was not found.");
            }

            var existingAccountAdmin = await unitOfWork.Admins.GetAsync(x => x.AccountId == dto.AccountId, cancellationToken: ct);
            if (existingAccountAdmin is not null)
            {
                logger.LogWarning("Admin for account {AccountId} already exists.", dto.AccountId);
                throw new InvalidOperationException($"Admin with AccountId {dto.AccountId} already exists.");
            }

            if (account.Role != AccountRole.Admin)
            {
                logger.LogWarning("Account {AccountId} is not an admin.", dto.AccountId);
                throw new InvalidOperationException($"Account with ID {dto.AccountId} is not an admin.");
            }

            var admin = dto.ToEntity();

            unitOfWork.Admins.Add(admin);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            logger.LogInformation("Created admin {AdminId} for account {AccountId}.", admin.Id, admin.AccountId);
            return admin.ToDto();
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Creating admin failed.");
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating admin.");
        }
    }

    public async Task UpdateAdminAsync(UpdateAdminDto dto, Guid accountId, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var admin = await unitOfWork.Admins.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (admin is null)
            {
                logger.LogWarning("Admin with id {AdminId} does not exist.", dto.Id);
                throw new KeyNotFoundException($"Admin with ID {dto.Id} was not found.");
            }

            if (admin.AccountId != accountId)
            {
                logger.LogWarning("Account {AccountId} does not own admin {AdminId}.", accountId, dto.Id);
                throw new InvalidOperationException("You are not the owner of this admin record.");
            }

            unitOfWork.Admins.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation("Updated admin {AdminId}.", dto.Id);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Updating admin failed.");
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
            {
                logger.LogWarning("Admin with id {AdminId} does not exist.", id);
                throw new KeyNotFoundException($"Admin with ID {id} was not found.");
            }

            unitOfWork.Admins.Remove(admin);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation("Removed admin {AdminId}.", admin.Id);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Removing admin failed.");
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while removing admin.");
        }
    }

    public async Task<List<AdminDto>> CreateRangeAsync(List<CreateAdminDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any())
        {
            logger.LogInformation("Created {Count} admins.", 0);
            return [];
        }

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var accountIds = dtos.Select(x => x.AccountId);
            var existingAdmins = await unitOfWork.Admins.GetAllAsync(x => accountIds.Contains(x.AccountId), cancellationToken: ct);

            if (existingAdmins.Any())
            {
                var existingIds = existingAdmins.Select(a => a.AccountId);
                logger.LogWarning("Admins for accounts {AccountIds} already exist.", existingIds);
                throw new InvalidOperationException($"Admins with AccountIds already exist: {string.Join(", ", existingIds)}");
            }

            var entities = dtos.Select(dto => dto.ToEntity());

            unitOfWork.Admins.AddRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            var created = entities.Select(admin => admin.ToDto()).ToList();
            logger.LogInformation("Created {Count} admins.", created.Count);
            return created;
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Creating admins failed.");
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new ServerException("An error occurred while creating admins.");
        }
    }

    public async Task UpdateRangeAsync(List<UpdateAdminDto> dtos, CancellationToken ct = default)
    {
        var uniqueDtos = dtos.DistinctBy(d => d.Id);
        if (!uniqueDtos.Any())
        {
            logger.LogInformation("Updated {Count} admins.", 0);
            return;
        }

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var ids = uniqueDtos.Select(d => d.Id);

            var existingAdmins = await unitOfWork.Admins.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);

            if (existingAdmins.Count != uniqueDtos.Count())
            {
                logger.LogWarning("One or more admins do not exist.");
                throw new KeyNotFoundException("One or more admins were not found.");
            }

            var entities = uniqueDtos.Select(d => d.UpdateEntity());

            unitOfWork.Admins.UpdateRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation("Updated {Count} admins.", uniqueDtos.Count());
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Updating admins failed.");
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while updating admins.");
        }
    }

    public async Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct();
        if (!distinctIds.Any())
        {
            logger.LogInformation("Removed {Count} admins.", 0);
            return;
        }

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var existingAdmins = await unitOfWork.Admins.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct);

            if (existingAdmins.Count != distinctIds.Count())
            {
                logger.LogWarning("One or more admins do not exist.");
                throw new KeyNotFoundException("One or more admins were not found.");
            }

            unitOfWork.Admins.RemoveRange(existingAdmins);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
            logger.LogInformation("Removed {Count} admins.", existingAdmins.Count);
        }
        catch (Exception ex) when (ex is not AppException)
        {
            logger.LogError(ex, "Removing admins failed.");
            await transaction.RollbackAsync(ct);
            throw new ServerException("An error occurred while removing admins.");
        }
    }

}