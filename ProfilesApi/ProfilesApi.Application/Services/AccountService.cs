using ProfilesApi.Application.DTOs.Common;
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

public sealed class AccountService(IUnitOfWork unitOfWork) : IAccountService
{
    private static Expression<Func<AccountEntity, bool>> BuildFilterExpression(AccountFilterDto? filter)
    {
        if (filter is null)
            return x => true;

        var searchTerm = filter.SearchTerm?.Trim();

        return x => (filter.Role == null || x.Role == filter.Role)
                 && (string.IsNullOrWhiteSpace(filter.Email) || x.Email.Contains(filter.Email))
                 && (string.IsNullOrWhiteSpace(filter.PhoneNumber) || x.PhoneNumber.Contains(filter.PhoneNumber))
                 && (string.IsNullOrWhiteSpace(searchTerm) ||
                     x.Email.Contains(searchTerm) ||
                     x.PhoneNumber.Contains(searchTerm));
    }

    public async Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken ct = default)
    {
        var account = await unitOfWork.Accounts.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (account is null)
            throw new KeyNotFoundException($"Account with ID {id} was not found.");

        return account.ToDto();
    }

    public async Task<PagedResult<AccountDto>> GetAccountsAsync(
        AccountFilterDto? filter,
        PaginationParams paginationParams,
        CancellationToken ct = default)
    {
        paginationParams ??= new PaginationParams();
        var filterExpression = BuildFilterExpression(filter);

        var (accounts, totalCount) = await unitOfWork.Accounts.GetAllAsync(
            filterExpression,
            paginationParams.PageNumber,
            paginationParams.PageSize,
            cancellationToken: ct);

        var dtos = accounts.Select(account => account.ToDto());

        return new PagedResult<AccountDto>(dtos, totalCount, paginationParams.PageNumber, paginationParams.PageSize);
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber, cancellationToken: ct);
            if (account is not null)
                throw new Exception($"Account with Email {dto.Email} or Phone Number {dto.PhoneNumber} already exists.");

            account = dto.ToEntity();

            unitOfWork.Accounts.Add(account);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return account.ToDto();
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while creating accounts.");
        }
    }

    public async Task UpdateAccountAsync(UpdateAccountDto dto, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == dto.Id, cancellationToken: ct);
            if (account is null)
                throw new KeyNotFoundException($"Account with ID {dto.Id} was not found.");

            unitOfWork.Accounts.Update(dto.UpdateEntity());
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while updating accounts.");
        }
    }

    public async Task RemoveAccountAsync(Guid id, CancellationToken ct = default)
    {
        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var account = await unitOfWork.Accounts.GetAsync(x => x.Id == id, true, cancellationToken: ct);
            if (account is null)
                throw new KeyNotFoundException($"Account with ID {id} was not found.");

            unitOfWork.Accounts.Remove(account);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while removing accounts.");
        }
    }

    public async Task<IEnumerable<AccountDto>> CreateRangeAsync(IEnumerable<CreateAccountDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var emails = dtos.Select(x => x.Email).ToHashSet();
            var phones = dtos.Select(x => x.PhoneNumber).ToHashSet();

            var (existingAccounts, _) = await unitOfWork.Accounts.GetAllAsync(
                x => emails.Contains(x.Email) || phones.Contains(x.PhoneNumber),
                cancellationToken: ct
            );

            if (existingAccounts.Any())
                throw new InvalidOperationException("An account with one of the provided emails or phone numbers already exists.");

            var entities = dtos.Select(dto => dto.ToEntity());

            unitOfWork.Accounts.AddRange(entities);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return entities.Select(account => account.ToDto());
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while creating accounts.");
        }
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateAccountDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var uniqueDtos = dtos.DistinctBy(d => d.Id).ToList();
            var ids = uniqueDtos.Select(d => d.Id).ToList();

            var (existingAccounts, _) = await unitOfWork.Accounts.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);
            var existingList = existingAccounts.ToList();

            if (existingList.Count != uniqueDtos.Count)
                throw new KeyNotFoundException("One or more accounts were not found.");

            var entities = uniqueDtos.Select(d => d.UpdateEntity());
            unitOfWork.Accounts.UpdateRange(entities);

            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while updating accounts.");
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var distinctIds = ids.Distinct().ToList();
        if (distinctIds.Count == 0) return;

        using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken: ct);
        try
        {
            var (existingAccounts, _) = await unitOfWork.Accounts.GetAllAsync(x => distinctIds.Contains(x.Id), cancellationToken: ct);
            var existingList = existingAccounts.ToList();

            if (existingList.Count != distinctIds.Count)
                throw new KeyNotFoundException("One or more accounts were not found.");

            unitOfWork.Accounts.RemoveRange(existingList);
            await unitOfWork.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw new Exception("An error occurred while removing accounts.");
        }
    }
}
