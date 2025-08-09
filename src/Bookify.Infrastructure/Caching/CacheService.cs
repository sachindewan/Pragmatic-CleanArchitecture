using Bookify.Application.Abstractions.Caching;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Caching
{
    internal sealed class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache ?? throw new ArgumentNullException(nameof(distributedCache));
        }
        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            byte[]? bytes = await _distributedCache.GetAsync(key);

            return bytes is null ? default : Desearialize<T>(bytes);
        }


        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(key, cancellationToken);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            byte[] bytes = Searialize(value);
          
            return _distributedCache.SetAsync(key, bytes, CacheOptions.Create(expiration), cancellationToken);
        }

        private static T Desearialize<T>(byte[] bytes)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(bytes) ?? throw new InvalidOperationException("Deserialization failed.");
        }

        private static byte[] Searialize<T>(T value)
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value) ?? throw new InvalidOperationException("Serialization failed.");
        }
    }
}
