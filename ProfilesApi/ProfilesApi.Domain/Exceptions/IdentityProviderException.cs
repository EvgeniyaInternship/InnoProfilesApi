namespace ProfilesApi.Domain.Exceptions;

public sealed class IdentityProviderException(string message, Exception? innerException = null)
    : AppException(message, innerException);
