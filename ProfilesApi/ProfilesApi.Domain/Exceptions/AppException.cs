namespace ProfilesApi.Domain.Exceptions;

public abstract class AppException(string message, Exception? innerException = null) : Exception(message, innerException);
