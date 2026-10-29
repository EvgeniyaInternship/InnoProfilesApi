using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Api.Authorization;
using ProfilesApi.Domain.Enum;
using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AccountController(IAccountService accountService) : ControllerBase
{
    [Authorize(Policy = Policies.AnyRole)]
    [OwnResource(ProfileSubject.Account, AccountRole.Doctor, AccountRole.Receptionist)]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var account = await accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(account);
    }

    [Authorize(Policy = Policies.Staff)]
    [HttpGet]
    [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccounts([FromQuery] AccountFilterDto? filter, [FromQuery] PaginationParamsDto paginationParams, CancellationToken cancellationToken)
    {
        var pagedAccounts = await accountService.GetAccountsAsync(paginationParams, filter, cancellationToken);
        return Ok(pagedAccounts);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        var account = await accountService.CreateAccountAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
    }

    [Authorize(Policy = Policies.AnyRole)]
    [OwnResource(ProfileSubject.Account)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDto dto, CancellationToken cancellationToken)
    {
        if (!User.IsInRole(AccountRole.Admin.ToString()) && !User.IsInRole(dto.Role.ToString()))
            return Forbid();

        await accountService.UpdateAccountAsync(dto, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAccount([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await accountService.RemoveAccountAsync(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost("batch")]
    [ProducesResponseType(typeof(List<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateAccountDto> dtos, CancellationToken cancellationToken)
    {
        var accounts = await accountService.CreateRangeAsync(dtos, cancellationToken);
        return Ok(accounts);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRange([FromBody] List<UpdateAccountDto> dtos, CancellationToken cancellationToken)
    {
        await accountService.UpdateRangeAsync(dtos, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpDelete("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRange([FromBody] List<Guid> ids, CancellationToken cancellationToken)
    {
        await accountService.RemoveRangeAsync(ids, cancellationToken);
        return NoContent();
    }
}