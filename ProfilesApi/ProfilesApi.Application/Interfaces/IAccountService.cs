using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;

namespace ProfilesApi.Application.Interfaces;

public interface IAccountService
{
    Task<AccountDto> GetAccountByIdAsync(Guid id, CancellationToken ct = default);
    Task<PagedResultDto<AccountDto>> GetAccountsAsync(
        PaginationParamsDto paginationParams, 
        AccountFilterDto? filter = null, 
        CancellationToken ct = default);
    Task<AccountDto> CreateAccountAsync(CreateAccountDto dto, CancellationToken ct = default);
    Task UpdateAccountAsync(UpdateAccountDto dto, CancellationToken ct = default);
    Task RemoveAccountAsync(Guid id, CancellationToken ct = default);
    Task<List<AccountDto>> CreateRangeAsync(List<CreateAccountDto> dtos, CancellationToken ct = default);
    Task UpdateRangeAsync(List<UpdateAccountDto> dtos, CancellationToken ct = default);
    Task RemoveRangeAsync(List<Guid> ids, CancellationToken ct = default);
}
