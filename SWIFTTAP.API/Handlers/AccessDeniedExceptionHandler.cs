using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Handlers;

internal sealed class AccessDeniedExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AccessDeniedExceptionHandler> _logger;

    public AccessDeniedExceptionHandler(ILogger<AccessDeniedExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AccessDeniedException ex)
            return false;

        _logger.LogError(ex, "Access denied from {IpAddress} with code: {Code}",
            httpContext.Request.GetIpAddress(), ex.ExceptionCode);

        var problemDetails = CreateProblemDetails(ex);

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private static ProblemDetails CreateProblemDetails(AccessDeniedException ex)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Access denied.",
            Detail = ex.Message
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = ex.ExceptionCode;

        return details;
    }
}


