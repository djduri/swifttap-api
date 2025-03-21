using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;

namespace SWIFTTAP.API.Handlers;

internal sealed class EntityDeleteExceptionHandler : IExceptionHandler
{
    private readonly ILogger<EntityDeleteExceptionHandler> _logger;

    public EntityDeleteExceptionHandler(ILogger<EntityDeleteExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not EntityDeleteException entityDeleteException)
            return false;

        LogEntityDeleteFailure(entityDeleteException);

        var problemDetails = CreateProblemDetails(entityDeleteException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogEntityDeleteFailure(EntityDeleteException entityDeleteException)
    {
        _logger.LogError(entityDeleteException, "Entity delete failed with code: {Code} ({@Exception})",
            entityDeleteException.ExceptionCode, entityDeleteException);
    }

    private static ProblemDetails CreateProblemDetails(EntityDeleteException entityDeleteException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Entity deletion failed.",
            Detail = entityDeleteException.Message,
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = entityDeleteException.ExceptionCode;

        return details;
    }
}
