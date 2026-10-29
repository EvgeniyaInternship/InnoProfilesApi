using System.Security.Claims;

namespace ProfilesApi.Api.Authorization;

public static class CurrentUser
{
    public static bool TryGetAccountId(ClaimsPrincipal user, out Guid accountId)
    {
        var value = user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(value, out accountId);
    }
}
