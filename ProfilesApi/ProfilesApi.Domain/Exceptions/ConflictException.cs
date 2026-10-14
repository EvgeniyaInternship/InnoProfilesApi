namespace ProfilesApi.Domain.Exceptions;

public sealed class ConflictException(string message) : AppException(message) { }
