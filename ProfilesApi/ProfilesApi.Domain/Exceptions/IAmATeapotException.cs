namespace ProfilesApi.Domain.Exceptions;

public sealed class IAmATeapotException(string message) : AppException(message) { }
