using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.DTOs.Filters;
using ProfilesApi.Application.DTOs.Requests.CreateRequests;
using ProfilesApi.Application.DTOs.Requests.UpdateRequests;
using ProfilesApi.Application.DTOs.Responses;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ReceptionistController(IReceptionistService receptionistService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ReceptionistDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReceptionistById(Guid id, CancellationToken cancellationToken)
    {
        var receptionist = await receptionistService.GetReceptionistByIdAsync(id, cancellationToken);
        return Ok(receptionist);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ReceptionistDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReceptionists([FromQuery] ReceptionistFilterDto? filter, CancellationToken cancellationToken)
    {
        var receptionists = await receptionistService.GetReceptionistsAsync(filter, cancellationToken);
        return Ok(receptionists);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ReceptionistDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateReceptionist([FromBody] CreateReceptionistDto dto, CancellationToken cancellationToken)
    {
        var receptionist = await receptionistService.CreateReceptionistAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetReceptionistById), new { id = receptionist.Id }, receptionist);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateReceptionist(Guid id, [FromBody] UpdateReceptionistDto dto, CancellationToken cancellationToken)
    {
        if (id != dto.Id)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Detail = "The Route ID and Body DTO ID must match."
            });
        }

        await receptionistService.UpdateReceptionistAsync(dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveReceptionist(Guid id, CancellationToken cancellationToken)
    {
        await receptionistService.RemoveReceptionistAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpPost("batch")]
    [ProducesResponseType(typeof(IEnumerable<ReceptionistDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRange([FromBody] IEnumerable<CreateReceptionistDto> dtos, CancellationToken cancellationToken)
    {
        var receptionists = await receptionistService.CreateRangeAsync(dtos, cancellationToken);
        return Ok(receptionists);
    }

    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRange([FromBody] IEnumerable<UpdateReceptionistDto> dtos, CancellationToken cancellationToken)
    {
        await receptionistService.UpdateRangeAsync(dtos, cancellationToken);
        return NoContent();
    }

    [HttpDelete("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveRange([FromBody] IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
        await receptionistService.RemoveRangeAsync(ids, cancellationToken);
        return NoContent();
    }
}
