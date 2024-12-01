using Microsoft.Extensions.Caching.Memory;
using Simple.Exchange.Shared.Dtos.Shared;
using Simple.Exchange.Shared.Interfaces;

namespace Simple.Exchange.Shared.Services;
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItemCallback) where T: CacheItem
    {
        if(_memoryCache.TryGetValue(key, out T? value) && value != null)
        {
            return value;
        }

        value = await getItemCallback();

        _memoryCache.Set(key, value, DateTimeOffset.FromUnixTimeMilliseconds(value.UnixTimestamp));

        return value;
    }
}