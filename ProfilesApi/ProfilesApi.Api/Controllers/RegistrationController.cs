using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.DTOs.Responses.Registration;
using ProfilesApi.Application.Interfaces;

namespace ProfilesApi.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class RegistrationController(IRegistrationService registrationService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(RegisteredUserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto, CancellationToken cancellationToken)
    {
        var registeredUser = await registrationService.RegisterAsync(dto, cancellationToken);
        return new ObjectResult(registeredUser)
        {
            StatusCode = StatusCodes.Status201Created,
            DeclaredType = typeof(RegisteredUserDto)
        };
    }
}
