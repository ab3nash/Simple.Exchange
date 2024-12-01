namespace Simple.Exchange.Shared.Dtos.Application;

public record CurrencyExchangeRequest(decimal Amount, string InputCurrency, string OutputCurrency);