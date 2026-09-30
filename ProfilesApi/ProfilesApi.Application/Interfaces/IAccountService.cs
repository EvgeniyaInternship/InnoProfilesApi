using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<AccountDto>> GetAllAccountsAsync(CancellationToken ct = default);
    Task<AccountDto> CreateAccountAsync(CreateAccountDto dto, CancellationToken ct = default);
    Task UpdateAccountAsync(UpdateAccountDto dto, CancellationToken ct = default);
    Task RemoveAccountAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<AccountDto>> CreateRangeAsync(IEnumerable<CreateAccountDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(IEnumerable<UpdateAccountDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(IEnumerable<Guid> ids, CancellationToken ct = default);
}
