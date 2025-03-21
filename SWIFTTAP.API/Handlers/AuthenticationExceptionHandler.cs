using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SWIFTTAP.Application.Exceptions;
using SWIFTTAP.Application.Extensions;

namespace SWIFTTAP.API.Handlers;

internal sealed class AuthenticationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<AuthenticationExceptionHandler> _logger;

    public AuthenticationExceptionHandler(ILogger<AuthenticationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AuthenticationException authException)
            return false;

        LogAuthenticationFailure(httpContext, authException);

        var problemDetails = CreateProblemDetails(authException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogAuthenticationFailure(HttpContext httpContext, AuthenticationException authException)
    {
        _logger.LogError(authException, "Authentication failure from host: {IpAddress} with code: {Code}",
            httpContext.Request.GetIpAddress(), authException.ExceptionCode);
    }

    private static ProblemDetails CreateProblemDetails(AuthenticationException authException)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status403Forbidden,
            Title = "Authentication failure.",
            Detail = authException.Message
        };

        details.Extensions[nameof(CodedException.ExceptionCode)] = authException.ExceptionCode;

        return details;
    }
}

