using Bookify.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behavior;

public class LoggingBehavior<TRequest, TResponse>(ILogger<TRequest> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    private readonly ILogger<TRequest> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        var name = request.GetType().Name;

        try
        {
            _logger.LogInformation("Executing command {Command}", name);

            var result = await next();

            _logger.LogInformation("Command {Command} processed successfully", name);

            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Command {Command} processing failed", name);
            throw;
        }
    }
}