using Domain.Interfaces.Common;
using EasyCaching.Core;

namespace Cache;

public class CacheRedis : ICache
{
    private readonly IEasyCachingProvider _redisCache;
    public CacheRedis(IEasyCachingProviderFactory factory)
    {
        _redisCache = factory.GetCachingProvider("RedisCache");
    }

    public async Task AddToCacheAsync(string cacheKey, object cacheObject, int minutesTimeOut)
    {
        await _redisCache.SetAsync(cacheKey, cacheObject, TimeSpan.FromMinutes(minutesTimeOut));
    }
    public async Task RemoveFromCacheAsync(string cacheKey)
    {
        await _redisCache.RemoveAsync(cacheKey);
    }
    public async Task UpdateCacheAsync(string cacheKey, object cacheObject, int minutesTimeOut)
    {
        await _redisCache.SetAsync(cacheKey, cacheObject, TimeSpan.FromMinutes(minutesTimeOut));
    }
    public async Task<bool> ExitsAsync(string cacheKey)
    {
        var result = await _redisCache.ExistsAsync(cacheKey);
        return result;
    }
    public async Task<T> GetObjectAsync<T>(string cacheKey)
    {
        var result = await _redisCache.GetAsync<T>(cacheKey);
        return result.Value;
    }
}
