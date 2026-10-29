using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Application.DTOs.Requests.Registration;
using ProfilesApi.Application.Interfaces;
using ProfilesApi.Domain.Exceptions;
using System.Net;

namespace ProfilesApi.Api.Clients;

public sealed class AuthIdentityClient(HttpClient httpClient) : IIdentityClient
{
    public async Task<Guid> RegisterAsync(IdentityRegistrationRequest request, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.PostAsJsonAsync("Auth/register", request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            var userId = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
            if (userId == Guid.Empty)
                throw new IdentityProviderException("Identity provider returned an empty user id.");

            return userId;
        }

        throw await CreateExceptionAsync(response, cancellationToken);
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.DeleteAsync($"Auth/users/{userId}", cancellationToken);
        if (response.IsSuccessStatusCode)
            return;

        throw await CreateExceptionAsync(response, cancellationToken);
    }

    private static async Task<Exception> CreateExceptionAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var message = await ReadMessageAsync(response, cancellationToken);
        return response.StatusCode switch
        {
            HttpStatusCode.Conflict => new ConflictException(message),
            HttpStatusCode.BadRequest => new ArgumentException(message),
            _ => new IdentityProviderException(message)
        };
    }

    private static async Task<string> ReadMessageAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var problem = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken);
            if (!string.IsNullOrWhiteSpace(problem?.Detail))
                return problem.Detail;
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
        }

        return response.ReasonPhrase ?? "Identity provider request failed.";
    }
}
