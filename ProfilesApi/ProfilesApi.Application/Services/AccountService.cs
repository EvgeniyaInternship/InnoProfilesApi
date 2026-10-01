using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Application.Mappings;
using ProfilesApi.Domain.Entities;
using ProfilesApi.Domain.Interfaces;

namespace ProfilesApi.Application.Services;

public class AccountService(IUnitOfWork unitOfWork) : IAccountService
{
    public async Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken ct = default)
    {
        var account = await unitOfWork.Accounts.GetAsync(x => x.Id == id, cancellationToken: ct);
        if (account is null)
            throw new KeyNotFoundException($"Account with ID {id} was not found.");

        return account.ToDto(); 
    }

    public async Task<IEnumerable<AccountDto>> GetAllAccountsByIdsAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var accounts = await unitOfWork.Accounts.GetAllAsync(x => ids.Contains(x.Id), cancellationToken: ct);
        return accounts.Select(account => account.ToDto()); 
    }

    public async Task<AccountDto> CreateAccountAsync(CreateAccountDto dto, CancellationToken ct = default)
    {
        var account = await unitOfWork.Accounts.GetAsync(x => x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber, cancellationToken: ct);
        if (account is not null)
            throw new Exception($"Account with Email {dto.Email} or Phone Number {dto.PhoneNumber} already exists.");

        account = dto.ToEntity();

        unitOfWork.Accounts.Add(account); 
        unitOfWork.SaveChanges(); 

        return account.ToDto();
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
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw;
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
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken: ct);
            throw;
        }
    }

    public async Task<IEnumerable<AccountDto>> CreateRangeAsync(IEnumerable<CreateAccountDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return [];

        var emails = dtos.Select(x => x.Email).ToHashSet();
        var phones = dtos.Select(x => x.PhoneNumber).ToHashSet();

        var existingAccounts = await unitOfWork.Accounts.GetAllAsync(
            x => emails.Contains(x.Email) || phones.Contains(x.PhoneNumber),
            cancellationToken: ct
        );

        if (existingAccounts.Any())
            throw new InvalidOperationException("An account with one of the provided emails or phone numbers already exists.");

        var entities = dtos.Select(dto => dto.ToEntity());  

        unitOfWork.Accounts.AddRange(entities); 
        await unitOfWork.SaveChangesAsync(ct); 

        return entities.Select(account => account.ToDto());
    }

    public async Task UpdateRangeAsync(IEnumerable<UpdateAccountDto> dtos, CancellationToken ct = default)
    {
        if (!dtos.Any()) return;
        var uniqueDtos = dtos.DistinctBy(d => d.Id);
        var entities = uniqueDtos.Select(d => d.UpdateEntity());

        unitOfWork.Accounts.UpdateRange(entities);

        await unitOfWork.SaveChangesAsync(ct); 
    }

    public async Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default)
    {
        var entitiesToDelete = ids.Distinct().Select(id => new AccountEntity { Id = id });
        if (!entitiesToDelete.Any()) return;

        unitOfWork.Accounts.RemoveRange(entitiesToDelete); 
        await unitOfWork.SaveChangesAsync(ct); 
    }
}
