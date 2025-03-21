using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SWIFTTAP.Application.Abstractions;

namespace SWIFTTAP.Application.Pipelines;

internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public RequestLoggingPipelineBehavior(IWebHostEnvironment webHostEnvironment,
                                          ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _webHostEnvironment = webHostEnvironment;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_webHostEnvironment.IsDevelopment())
        {
            return await next();
        }

        try
        {
            return await next();
        }
        catch
        {
            TryLogRequest(request);

            throw;
        }
    }

    private void TryLogRequest(TRequest request)
    {
        try
        {
            if (request is ICommand<TResponse> commandRequest)
            {
                _logger.LogError("Command of type {CommandType} failed with the payload: {@CommandPayload}",
                    commandRequest.GetType().Name,
                    commandRequest);

                return;
            }

            if (request is IQuery<TResponse> queryRequest)
            {
                _logger.LogError("Query of type {QueryType} failed with the payload: {@QueryPayload}",
                    queryRequest.GetType().Name,
                    request);

                return;
            }
        }
        catch
        {
            _logger.LogWarning("Failed to log the command or query intercepted payload");
        }
    }
}
