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
[Authorize(Policy = Policies.ReadDoctors)]
[Route("[controller]")]
public sealed class DoctorController(IDoctorService doctorService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDoctorById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var doctor = await doctorService.GetDoctorByIdAsync(id, cancellationToken);
        return Ok(doctor);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<DoctorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDoctors([FromQuery] DoctorFilterDto? filter, [FromQuery] PaginationParamsDto paginationParams, CancellationToken cancellationToken)
    {
        var doctors = await doctorService.GetDoctorsAsync(paginationParams, filter, cancellationToken);
        return Ok(doctors);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDoctor([FromBody] CreateDoctorDto dto, CancellationToken cancellationToken)
    {
        var doctor = await doctorService.CreateDoctorAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.Id }, doctor);
    }

    [Authorize(Policy = Policies.UpdateDoctor)]
    [OwnResource(ProfileSubject.Doctor)]
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateDoctor([FromBody] UpdateDoctorDto dto, CancellationToken cancellationToken)
    {
        await doctorService.UpdateDoctorAsync(dto, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveDoctor([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await doctorService.RemoveDoctorAsync(id, cancellationToken);
        return NoContent();
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPost("batch")]
    [ProducesResponseType(typeof(List<DoctorDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateRange([FromBody] List<CreateDoctorDto> dtos, CancellationToken cancellationToken)
    {
        var doctors = await doctorService.CreateRangeAsync(dtos, cancellationToken);
        return Ok(doctors);
    }

    [Authorize(Policy = Policies.Admin)]
    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateRange([FromBody] List<UpdateDoctorDto> dtos, CancellationToken cancellationToken)
    {
        await doctorService.UpdateRangeAsync(dtos, cancellationToken);
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
        await doctorService.RemoveRangeAsync(ids, cancellationToken);
        return NoContent();
    }
}