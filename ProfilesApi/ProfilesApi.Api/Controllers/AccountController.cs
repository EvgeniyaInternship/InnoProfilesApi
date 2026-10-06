using Microsoft.AspNetCore.Mvc;
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
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccountById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var account = await accountService.GetAccountByIdAsync(id, cancellationToken);
        return Ok(account);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccounts([FromQuery] AccountFilterDto? filter, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var pagedAccounts = await accountService.GetAccountsAsync(filter, paginationParams, cancellationToken);
        return Ok(pagedAccounts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto, CancellationToken cancellationToken)
    {
        var account = await accountService.CreateAccountAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAccountById), new { id = account.Id }, account);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateAccountDto dto, CancellationToken cancellationToken)
    {
        await accountService.UpdateAccountAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAccount([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await accountService.RemoveAccountAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("batch")]
    [ProducesResponseType(typeof(IEnumerable<AccountDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRange([FromBody] IEnumerable<CreateAccountDto> dtos, CancellationToken cancellationToken)
    {
        var accounts = await accountService.CreateRangeAsync(dtos, cancellationToken);
        return Ok(accounts);
    }

    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRange([FromBody] IEnumerable<UpdateAccountDto> dtos, CancellationToken cancellationToken)
    {
        await accountService.UpdateRangeAsync(dtos, cancellationToken);
        return NoContent();
    }

    [HttpDelete("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRange([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await accountService.RemoveRangeAsync(ids, cancellationToken);
        return NoContent();
    }
}