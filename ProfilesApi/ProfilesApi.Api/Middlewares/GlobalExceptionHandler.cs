using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProfilesApi.Domain.Exceptions;
using System.Text.Json;

namespace ProfilesApi.Api.Middlewares;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            InvalidFormatException or ArgumentException or ArgumentNullException =>
                (StatusCodes.Status400BadRequest, "Invalid Request"),

            ConflictException =>
                (StatusCodes.Status400BadRequest, "Resource Conflict"),

            UnauthorizedAccessException =>
                (StatusCodes.Status401Unauthorized, "Unauthorized"),

            KeyNotFoundException =>
                (StatusCodes.Status404NotFound, "Resource Not Found"),

            IAmATeapotException =>
                (StatusCodes.Status418ImATeapot, "I'm a Teapot (рэспект)"),

            ServerException =>
                (StatusCodes.Status500InternalServerError, "Internal Server Error"),

            _ =>
                (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            logger.LogError(
                exception,
                "Unhandled server exception | Method: {Method} | Path: {Path} | Status: {StatusCode}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                statusCode);
        }
        else
        {
            logger.LogWarning(
                "Client error encountered | Method: {Method} | Path: {Path} | Status: {StatusCode} | Message: {Message}",
                httpContext.Request.Method,
                httpContext.Request.Path,
                statusCode,
                exception.Message);
        }

        var problemDetails = new ProblemDetails
        {
            Type = exception.GetType().Name,
            Title = title,
            Status = statusCode,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred while processing your request."
                : exception.Message,
            Instance = httpContext.Request.Path,
            Extensions =
            {
                ["traceId"] = httpContext.TraceIdentifier,
                ["timestamp"] = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
            }
        };

        httpContext.Response.StatusCode = statusCode;
        httpContext.Response.ContentType = "application/problem+json";

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, jsonOptions, cancellationToken);

        return true;
    }
}