using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using Simple.Exchange.Domain.Exceptions;
using Simple.Exchange.Domain.Interfaces.Services;
using Simple.Exchange.Shared.Dtos.Configurations;
using Simple.Exchange.Shared.Dtos.Infrastructure;

namespace Simple.Exchange.Infrastructure.Services;
public class ExchangeRateService : IExchangeRateService
{
    private readonly ExchangeRateApiConfig _apiConfig;
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient, IOptions<ExchangeRateApiConfig> apiConfig)
    {
        _apiConfig = apiConfig.Value;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri(_apiConfig.BaseUrl!);
    }

    public async Task<ExchangeRateApiResponse> GetExchangeRatesAsync(string baseCode)
    {
        string requestUrl = string.Format(_apiConfig.LatestRateEndpoint!, _apiConfig.ApiKey, baseCode);
        HttpRequestMessage requestMessage = new(HttpMethod.Get, requestUrl);

        var response = await _httpClient.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            throw new ExchangeRateServiceException(
                response.StatusCode, _httpClient.BaseAddress!.ToString(), _apiConfig.LatestRateEndpoint!, responseContent);
        }

        var apiResponse = await response.Content.ReadFromJsonAsync<ExchangeRateApiResponse>();

        if (apiResponse == null)
        {
            throw new ExchangeRateServiceException("Api returned empty response.");
        }

        return apiResponse!;
    }
}