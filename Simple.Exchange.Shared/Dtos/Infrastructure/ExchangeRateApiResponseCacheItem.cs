using Simple.Exchange.Shared.Dtos.Shared;

namespace Simple.Exchange.Shared.Dtos.Infrastructure;
public class ExchangeRateApiResponseCacheItem : CacheItem
{
    public ExchangeRateApiResponseCacheItem(ExchangeRateApiResponse apiResponse)
    {
        ExchangeRateApiResponse = apiResponse;
    }

    public ExchangeRateApiResponse ExchangeRateApiResponse { get; set; }

    // Add 1s delay for update to be reflected, to be updated based on requirement
    public override long UnixTimestamp { get => ExchangeRateApiResponse.TimeNextUpdateUnix + 1; }
}
