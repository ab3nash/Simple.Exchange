using MediatR;
using Simple.Exchange.Application.Queries;

namespace Simple.Exchange.Api.Endpoints;

public static class CurrencyExchangeEndpoints
{
    public static IEndpointRouteBuilder MapExchangeEndpoints(this IEndpointRouteBuilder routes)
    {
        // This would be better suited as a GET request instead of POST
        routes.MapPost(
            "/ExchangeService",
            async (IMediator mediator, CurrencyExchangeQuery currencyExchangeQuery) => {
                return Results.Ok(await mediator.Send(currencyExchangeQuery));
            })
            .WithName("ExchangeService");

        return routes;
    }
}