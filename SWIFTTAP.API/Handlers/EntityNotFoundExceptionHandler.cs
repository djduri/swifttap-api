using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;

namespace SWIFTTAP.API.Handlers;

internal sealed class EntityNotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<EntityNotFoundExceptionHandler> _logger;

    public EntityNotFoundExceptionHandler(ILogger<EntityNotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not EntityNotFoundException entityNotFoundException)
            return false;

        LogEntityNotFoundFailure(entityNotFoundException);

        var problemDetails = CreateProblemDetails(entityNotFoundException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogEntityNotFoundFailure(EntityNotFoundException entityNotFoundException)
    {
        _logger.LogError(entityNotFoundException, "Entity not found with code: {Code} ({@Exception})",
            entityNotFoundException.ExceptionCode, entityNotFoundException);
    }

    private static ProblemDetails CreateProblemDetails(EntityNotFoundException entityNotFoundException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = "Entity not found.",
            Detail = entityNotFoundException.Message,
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = entityNotFoundException.ExceptionCode;

        return details;
    }
}