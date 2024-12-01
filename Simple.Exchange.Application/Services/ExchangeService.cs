using Simple.Exchange.Domain.Exceptions;
using Simple.Exchange.Domain.Interfaces.Services;
using Simple.Exchange.Shared.Dtos.Application;
using Simple.Exchange.Shared.Dtos.Infrastructure;
using Simple.Exchange.Shared.Interfaces;

namespace Simple.Exchange.Application.Services;
public class ExchangeService: IExchangeService
{
    private readonly IExchangeRateService _exchangeRateService;
    private readonly ICacheService _cacheService;
    private const string _exchangeRateCacheKey = "ExchangeRates";

    public ExchangeService(IExchangeRateService exchangeRateService, ICacheService cacheService)
    {
        _exchangeRateService = exchangeRateService;
        _cacheService = cacheService;
    }

    public async Task<CurrencyExchangeResponse> ExchangeAsync(CurrencyExchangeRequest currencyExchangeRequest)
    {
        var exchangeRates = await _cacheService.GetOrSetAsync(
            _exchangeRateCacheKey,
            () => GetExchangeRateApiResponse(currencyExchangeRequest.InputCurrency));

        if(exchangeRates.ExchangeRateApiResponse.ConversionRates?.ContainsKey(currencyExchangeRequest.OutputCurrency) != true)
        {
            throw new ExchangeServiceException($"Could not find exchange rate from " +
                $"{currencyExchangeRequest.InputCurrency} to {currencyExchangeRequest.OutputCurrency}");
        }

        var exchangeRate = exchangeRates.ExchangeRateApiResponse.ConversionRates[currencyExchangeRequest.OutputCurrency];
        var convertedValue = exchangeRate * currencyExchangeRequest.Amount;

        return new CurrencyExchangeResponse(
            currencyExchangeRequest.Amount,
            currencyExchangeRequest.InputCurrency,
            currencyExchangeRequest.OutputCurrency,
            convertedValue);
    }

    private async Task<ExchangeRateApiResponseCacheItem> GetExchangeRateApiResponse(string baseCurrencyCode)
    {
        var apiResponse = await _exchangeRateService.GetExchangeRatesAsync(baseCurrencyCode);
        return new ExchangeRateApiResponseCacheItem(apiResponse);
    }
}