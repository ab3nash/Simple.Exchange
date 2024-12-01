using MediatR;
using Simple.Exchange.Shared.Dtos.Application;

namespace Simple.Exchange.Application.Queries;
public class CurrencyExchangeQuery : IRequest<CurrencyExchangeResponse>
{
    public decimal Amount { get; init; }
    public string? InputCurrency { get; init; }
    public string? OutputCurrency { get; init;}
}