using Bookify.Application.Abstractions.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors
{
    internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IBaseCommand
    {
        private readonly ILogger<TRequest> _logger;
        public LoggingBehavior(ILogger<TRequest> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var name = request.GetType().Name;
            try
            {
                _logger.LogInformation("Executing command {command}", name);
                var response = await next();
                _logger.LogInformation("Command {command} processed successfully", name);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Command {command} processing failed", name);
                throw;
            }

        }
    }
}
