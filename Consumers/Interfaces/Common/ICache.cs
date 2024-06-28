namespace Domain.Interfaces.Common;

public interface ICache
{
    Task AddToCacheAsync(string cacheKey, object cacheObject, int minutesTimeOut);
    Task RemoveFromCacheAsync(string cacheKey);
    Task UpdateCacheAsync(string cacheKey, object cacheObject, int minutesTimeOut);
    Task<bool> ExitsAsync(string cacheKey);
    Task<T> GetObjectAsync<T>(string cacheKey);
}
