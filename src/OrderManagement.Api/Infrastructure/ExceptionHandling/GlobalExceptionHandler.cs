using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.Api.Domain.Common;

namespace OrderManagement.Api.Infrastructure.ExceptionHandling;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is DomainException domainException)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Business rule validation failed",
                Detail = domainException.Message,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] =
                httpContext.TraceIdentifier;

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }

        if (exception is ConflictException conflictException)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Resource conflict",
                Detail = conflictException.Message,
                Instance = httpContext.Request.Path
            };

            problemDetails.Extensions["traceId"] =
                httpContext.TraceIdentifier;

            httpContext.Response.StatusCode =
                StatusCodes.Status409Conflict;

            await httpContext.Response.WriteAsJsonAsync(
                problemDetails,
                cancellationToken);

            return true;
        }

        logger.LogError(
            exception,
            "An unhandled exception occurred while processing {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path);

        var internalError = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "An unexpected error occurred",
            Detail = "The server could not complete the request.",
            Instance = httpContext.Request.Path
        };

        internalError.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode =
            StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            internalError,
            cancellationToken);

        return true;
    }
}