using ProfilesApi.Application.DTOs.Requests.Registration;

namespace ProfilesApi.Application.Interfaces;

public interface IIdentityClient
{
    Task<Guid> RegisterAsync(IdentityRegistrationRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default);
}
