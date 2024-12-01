using Simple.Exchange.Shared.Dtos.Shared;

namespace Simple.Exchange.Shared.Interfaces;

public interface ICacheService
{
    Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback) where T : CacheItem;
}