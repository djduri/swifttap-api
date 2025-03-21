using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Handlers;

internal sealed class AuthorizationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AuthorizationExceptionHandler> _logger;

    public AuthorizationExceptionHandler(ILogger<AuthorizationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AuthorizationException authException)
            return false;

        LogAuthorizationFailure(httpContext, authException);

        var problemDetails = CreateProblemDetails(authException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogAuthorizationFailure(HttpContext httpContext, AuthorizationException authException)
    {
        _logger.LogError(authException, "Authorization failure from host: {IpAddress} with code: {Code}",
            httpContext.Request.GetIpAddress(), authException.ExceptionCode);
    }

    private static ProblemDetails CreateProblemDetails(AuthorizationException authException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Authorization failure",
            Detail = authException.Message,
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = authException.ExceptionCode;

        return details;
    }
}

