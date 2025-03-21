using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SWIFTTAP.API.Handlers;

internal sealed class UnhandledExceptionHandler : IExceptionHandler
{
    private readonly ILogger<UnhandledExceptionHandler> _logger;

    public UnhandledExceptionHandler(ILogger<UnhandledExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Log the exception with message and details
        LogUnknownFailure(exception);

        var problemDetails = CreateProblemDetails(exception);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogUnknownFailure(Exception exception)
    {
        _logger.LogError(exception, "Unknown system failure with message: {Message} ({@Exception})",
            exception.Message, exception);
    }

    private static ProblemDetails CreateProblemDetails(Exception exception)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "System failure.",
            Detail = exception.Message,
        };

        return details;
    }
}
