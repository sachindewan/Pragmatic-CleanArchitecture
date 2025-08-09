using Microsoft.Extensions.Caching.Distributed;

namespace Bookify.Infrastructure.Caching
{
    public static class CacheOptions
    {
        public static DistributedCacheEntryOptions DistributedCacheEntryOptions => new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
        };

        public static DistributedCacheEntryOptions Create(TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null)
        {
            return absoluteExpirationRelativeToNow is not null || slidingExpiration is not null
                ? new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow,
                    SlidingExpiration = slidingExpiration
                }
                : DistributedCacheEntryOptions;
        }
    }
}
