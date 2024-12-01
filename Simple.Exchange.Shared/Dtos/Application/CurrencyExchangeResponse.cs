namespace Simple.Exchange.Shared.Dtos.Application;
public record CurrencyExchangeResponse(decimal Amount, string InputCurrency, string OutputCurrenct, decimal Value);