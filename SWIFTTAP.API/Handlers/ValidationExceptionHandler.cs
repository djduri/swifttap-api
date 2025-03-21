using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SWIFTTAP.API.Handlers;

internal sealed class ValidationExceptionHandler : IExceptionHandler
{
    private readonly ILogger<ValidationExceptionHandler> _logger;

    public ValidationExceptionHandler(ILogger<ValidationExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not ValidationException validationException)
        {
            return false;
        }

        // Log the validation failure details
        LogValidationFailure(validationException);

        // Prepare the problem details
        var problemDetails = CreateProblemDetails(validationException);

        // StatusCode will never be null, so we use !
        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }

    private void LogValidationFailure(ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray());

        _logger.LogInformation("Request fields validation failed with message: {Message} due to errors: {@Errors} ({@Exception})",
            validationException.Message,
            errors,
            validationException);
    }

    private static ValidationProblemDetails CreateProblemDetails(ValidationException validationException)
    {
        var errors = validationException.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(k => k.Key, v => v.Select(e => e.ErrorMessage).ToArray());

        var details = new ValidationProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "One or more validation errors occurred.",
            Errors = errors
        };

        return details;
    }
}
