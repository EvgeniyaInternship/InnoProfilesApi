using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Api.Authorization;
using ProfilesApi.Application.DTOs.Common;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Api.Controllers;

[ApiController]
[Authorize(Policy = Policies.Staff)]
[Route("[controller]")]
public sealed class ReceptionistController(IReceptionistService receptionistService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReceptionistDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReceptionistById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var receptionist = await receptionistService.GetReceptionistByIdAsync(id, cancellationToken);
        return Ok(receptionist);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReceptionistDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReceptionists([FromQuery] ReceptionistFilterDto? filter, [FromQuery] PaginationParamsDto paginationParams, CancellationToken cancellationToken)
    {
        var receptionists = await receptionistService.GetReceptionistsAsync(paginationParams, filter, cancellationToken);
        return Ok(receptionists);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(ReceptionistDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReceptionist([FromBody] CreateReceptionistDto dto, CancellationToken cancellationToken)
    {
        var receptionist = await receptionistService.CreateReceptionistAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetReceptionistById), new { id = receptionist.Id }, receptionist);
    }

    [Authorize(Policy = Policies.UpdateReceptionist)]
    [OwnResource(ProfileSubject.Receptionist)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateReceptionist([FromBody] UpdateReceptionistDto dto, CancellationToken cancellationToken)
    {
        await receptionistService.UpdateReceptionistAsync(dto, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveReceptionist([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await receptionistService.RemoveReceptionistAsync(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost("batch")]
    [ProducesResponseType(typeof(List<ReceptionistDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateReceptionistDto> dtos, CancellationToken cancellationToken)
    {
        var receptionists = await receptionistService.CreateRangeAsync(dtos, cancellationToken);
        return Ok(receptionists);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRange([FromBody] List<UpdateReceptionistDto> dtos, CancellationToken cancellationToken)
    {
        await receptionistService.UpdateRangeAsync(dtos, cancellationToken);
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
        await receptionistService.RemoveRangeAsync(ids, cancellationToken);
        return NoContent();
    }
}
