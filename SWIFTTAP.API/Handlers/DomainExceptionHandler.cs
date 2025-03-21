using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Domain.Base;

namespace SWIFTTAP.API.Handlers;

internal sealed class DomainExceptionHandler : IExceptionHandler
{
    private readonly ILogger<DomainExceptionHandler> _logger;

    public DomainExceptionHandler(ILogger<DomainExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DomainException domainException)
            return false;

        LogDomainFailure(domainException);

        var problemDetails = CreateProblemDetails(domainException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogDomainFailure(DomainException domainException)
    {
        _logger.LogError(domainException, "Domain layer logic failed with code: {Code} ({@Exception})",
            domainException.ExceptionCode, domainException);
    }

    private static ProblemDetails CreateProblemDetails(DomainException domainException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Domain logic failure",
            Detail = domainException.Message,
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = domainException.ExceptionCode;

        return details;
    }
}

