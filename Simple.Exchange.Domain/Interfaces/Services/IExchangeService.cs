using Simple.Exchange.Shared.Dtos.Application;

namespace Simple.Exchange.Domain.Interfaces.Services;
public interface IExchangeService
{
    Task<CurrencyExchangeResponse> ExchangeAsync(CurrencyExchangeRequest currencyExchangeRequest);
}