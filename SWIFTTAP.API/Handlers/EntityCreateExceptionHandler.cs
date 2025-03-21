using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;

namespace SWIFTTAP.API.Handlers;

internal sealed class EntityCreateExceptionHandler : IExceptionHandler
{
    private readonly ILogger<EntityCreateExceptionHandler> _logger;

    public EntityCreateExceptionHandler(ILogger<EntityCreateExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not EntityCreateException entityCreationException)
            return false;

        LogEntityCreateFailure(entityCreationException);

        var problemDetails = CreateProblemDetails(entityCreationException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogEntityCreateFailure(EntityCreateException entityCreationException)
    {
        _logger.LogError(entityCreationException, "Entity create failed with code: {Code} ({@Exception})",
            entityCreationException.ExceptionCode, entityCreationException);
    }

    private static ProblemDetails CreateProblemDetails(EntityCreateException entityCreationException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Entity creation failed.",
            Detail = entityCreationException.Message,
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = entityCreationException.ExceptionCode;

        return details;
    }
}

