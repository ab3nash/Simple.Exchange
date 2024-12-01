using System.Net;
using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using Simple.Exchange.Application.Behaviors;
using Simple.Exchange.Application.Queries;
using Simple.Exchange.Application.Services;
using Simple.Exchange.Application.Validators;
using Simple.Exchange.Domain.Interfaces.Services;
using Simple.Exchange.Infrastructure.Services;
using Simple.Exchange.Shared.Dtos.Configurations;
using Simple.Exchange.Shared.Interfaces;
using Simple.Exchange.Shared.Services;

namespace Simple.Exchange.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterDependencies(this IServiceCollection services)
    {
        services.AddScoped<IExchangeRateService, ExchangeRateService>();
        services.AddScoped<IExchangeService, ExchangeService>();

        services.AddMemoryCache();

        // a different cache may be used, but memorycache is enought given the scope
        services.AddSingleton<ICacheService, MemoryCacheService>();

        return services;
    }

    public static IServiceCollection RegisterMediatr(this IServiceCollection services)
    {
        var applicationAssembly = Assembly.GetAssembly(typeof(CurrencyExchangeQuery))!;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
        services.AddScoped<IValidator<CurrencyExchangeQuery>, CurrencyExchangeQueryValidator>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }

    public static IServiceCollection RegisterHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<ExchangeRateService>()
            .AddResilienceHandler("TransientErrorRetryPolicy", resilienceBuilder => {
                List<HttpStatusCode> statusCodesToRetry = [
                    HttpStatusCode.RequestTimeout,
            HttpStatusCode.BadGateway,
            HttpStatusCode.InternalServerError,
            HttpStatusCode.ServiceUnavailable
                ];

                // Add retry policy for above list of httpstatus codes
                // List to be modified based on requirement
                resilienceBuilder.AddRetry(new HttpRetryStrategyOptions {
                    MaxRetryAttempts = configuration.GetValue<int>("ExchangeRateApiConfig:MaxRetryCount"),
                    Delay = TimeSpan.FromSeconds(configuration.GetValue<int>("ExchangeRateApiConfig:RetryDelaySeconds")),
                    BackoffType = DelayBackoffType.Exponential,
                    UseJitter = true,
                    ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                        .HandleResult(response => response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                });
            });

        return services;
    }

    public static IServiceCollection InjectConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ExchangeRateApiConfig>(configuration.GetSection("ExchangeRateApiConfig"));
        services.Configure<CurrencyExchangeConfig>(configuration.GetSection("CurrencyExchangeConfig"));

        return services;
    }
}