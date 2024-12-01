using FluentAssertions;
using Moq;
using Simple.Exchange.Application.Services;
using Simple.Exchange.Domain.Exceptions;
using Simple.Exchange.Domain.Interfaces.Services;
using Simple.Exchange.Shared.Dtos.Application;
using Simple.Exchange.Shared.Dtos.Infrastructure;
using Simple.Exchange.Shared.Interfaces;

namespace Simple.Exchange.Application.UnitTests.Services;
public class ExchangeServiceTests
{
    private readonly Mock<IExchangeRateService> _exchangeRateServiceMock = new();
    private readonly Mock<ICacheService> _cacheServiceMock = new();
    private readonly ExchangeService _exchangeService;

    public ExchangeServiceTests()
    {
        _exchangeService = new ExchangeService(
            _exchangeRateServiceMock.Object,
            _cacheServiceMock.Object);
    }

    [Fact]
    public async Task ExchangeAsync_Should_ConvertCurrenty_If_ExchangeRateIsAvailable()
    {
        string inputCurrency = "NPR";
        string outputCurrency = "KRW";
        decimal rate = 20;
        decimal amount = 1000;

        var exchangeRateApiResponse = new ExchangeRateApiResponse {
            BaseCode = inputCurrency,
            ConversionRates = new Dictionary<string, decimal> { [outputCurrency] = rate }
        };

        ExchangeRateApiResponseCacheItem cacheItem = new(exchangeRateApiResponse);

        _cacheServiceMock.Setup(c => c.GetOrSetAsync(It.IsAny<string>(),
            It.IsAny<Func<Task<ExchangeRateApiResponseCacheItem>>>()))
            .ReturnsAsync(cacheItem);

        var request = new CurrencyExchangeRequest(amount, inputCurrency, outputCurrency);
        var expectedResponse = new CurrencyExchangeResponse(amount, inputCurrency, outputCurrency, 20000);


        var actualResponse = await _exchangeService.ExchangeAsync(request);

        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }

    [Fact]
    public async Task ExchangeAsync_Should_Throw_If_ExchangeRateIsNotAvailable()
    {
        string inputCurrency = "NPR";
        string outputCurrency = "KRW";
        decimal amount = 1000;

        var exchangeRateApiResponse = new ExchangeRateApiResponse {
            BaseCode = inputCurrency,
            ConversionRates = []
        };

        ExchangeRateApiResponseCacheItem cacheItem = new(exchangeRateApiResponse);

        _cacheServiceMock.Setup(c => c.GetOrSetAsync(It.IsAny<string>(),
            It.IsAny<Func<Task<ExchangeRateApiResponseCacheItem>>>()))
            .ReturnsAsync(cacheItem);

        var request = new CurrencyExchangeRequest(amount, inputCurrency, outputCurrency);

        await Assert.ThrowsAsync<ExchangeServiceException>(() => _exchangeService.ExchangeAsync(request));
    }
}
