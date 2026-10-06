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
public sealed class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AdminDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdminById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var admin = await adminService.GetAdminByIdAsync(id, cancellationToken);
        return Ok(admin);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AdminDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAdmins([FromQuery] AdminFilterDto? filter, [FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
    {
        var admins = await adminService.GetAdminsAsync(filter, paginationParams, cancellationToken);
        return Ok(admins);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AdminDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAdmin([FromBody] CreateAdminDto dto, CancellationToken cancellationToken)
    {
        var admin = await adminService.CreateAdminAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetAdminById), new { id = admin.Id }, admin);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAdmin([FromBody] UpdateAdminDto dto, CancellationToken cancellationToken)
    {
        await adminService.UpdateAdminAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveAdmin([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await adminService.RemoveAdminAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("batch")]
    [ProducesResponseType(typeof(IEnumerable<AdminDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRange([FromBody] IEnumerable<CreateAdminDto> dtos, CancellationToken cancellationToken)
    {
        var admins = await adminService.CreateRangeAsync(dtos, cancellationToken);
        return Ok(admins);
    }

    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRange([FromBody] IEnumerable<UpdateAdminDto> dtos, CancellationToken cancellationToken)
    {
        await adminService.UpdateRangeAsync(dtos, cancellationToken);
        return NoContent();
    }

    [HttpDelete("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveRange([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await adminService.RemoveRangeAsync(ids, cancellationToken);
        return NoContent();
    }
}
