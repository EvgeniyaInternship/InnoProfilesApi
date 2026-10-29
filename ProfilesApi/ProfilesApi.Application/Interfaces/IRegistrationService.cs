using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.DTOs.Responses.Registration;

namespace ProfilesApi.Application.Interfaces;

public interface IRegistrationService
{
    Task<RegisteredUserDto> RegisterAsync(RegisterUserDto dto, CancellationToken cancellationToken = default);
}
