using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Application.Abstractions.Behaviors
{
    internal sealed class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : ICachedQuery
        where TResponse : Result
    {
        private readonly ICacheService _cacheService;
        private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;
        public QueryCachingBehavior(ICacheService cacheService,ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
        {
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            TResponse? cachedResponse = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
            string name = typeof(TRequest).Name;
            if (cachedResponse is not null)
            {
                _logger.LogInformation($"Cache hit for {name} with key: {request.CacheKey}");
                return cachedResponse;
            }
            _logger.LogInformation($"Cache miss for {name} with key: {request.CacheKey}");
            var result = await next();
            if (result.IsSuccess)
            {
                await _cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
                _logger.LogInformation($"Cached {name} with key: {request.CacheKey} for {request.Expiration}");
            }
            return result;
        }
    }
}
