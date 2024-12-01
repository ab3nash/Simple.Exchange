using MediatR;
using Simple.Exchange.Application.Queries;
using Simple.Exchange.Domain.Interfaces.Services;
using Simple.Exchange.Shared.Dtos.Application;

namespace Simple.Exchange.Application.Handlers;
public class CurrencyExchangeQueryHandler : IRequestHandler<CurrencyExchangeQuery, CurrencyExchangeResponse>
{
    private readonly IExchangeService _exchangeService;

    public CurrencyExchangeQueryHandler(IExchangeService exchangeService)
    {
        _exchangeService = exchangeService;
    }

    public Task<CurrencyExchangeResponse> Handle(CurrencyExchangeQuery request, CancellationToken cancellationToken)
    {
        return _exchangeService.ExchangeAsync(
            new CurrencyExchangeRequest(request.Amount, request.InputCurrency!, request.OutputCurrency!));
    }
}