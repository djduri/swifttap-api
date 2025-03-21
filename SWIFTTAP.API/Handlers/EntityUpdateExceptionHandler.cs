using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;

namespace SWIFTTAP.API.Handlers;

internal sealed class EntityUpdateExceptionHandler : IExceptionHandler
{
    private readonly ILogger<EntityUpdateExceptionHandler> _logger;

    public EntityUpdateExceptionHandler(ILogger<EntityUpdateExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not EntityUpdateException entityUpdateException)
            return false;

        LogEntityUpdateFailure(entityUpdateException);

        var problemDetails = CreateProblemDetails(entityUpdateException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogEntityUpdateFailure(EntityUpdateException entityUpdateException)
    {
        _logger.LogError(entityUpdateException, "Entity update failed with code: {Code} ({@Exception})",
            entityUpdateException.ExceptionCode, entityUpdateException);
    }

    private static ProblemDetails CreateProblemDetails(EntityUpdateException entityUpdateException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Entity update failed.",
            Detail = entityUpdateException.Message,
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = entityUpdateException.ExceptionCode;

        return details;
    }
}
