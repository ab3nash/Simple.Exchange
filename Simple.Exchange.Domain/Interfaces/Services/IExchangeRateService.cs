using Simple.Exchange.Shared.Dtos.Infrastructure;

namespace Simple.Exchange.Domain.Interfaces.Services;
public interface IExchangeRateService
{
    public Task<ExchangeRateApiResponse> GetExchangeRatesAsync(string baseCode);
}