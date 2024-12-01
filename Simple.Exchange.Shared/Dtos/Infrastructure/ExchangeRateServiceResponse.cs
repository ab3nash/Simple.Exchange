using System.Text.Json.Serialization;

namespace Simple.Exchange.Shared.Dtos.Infrastructure;
public record ExchangeRateApiResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; init; }

    [JsonPropertyName("time_last_update_unix")]
    public long TimeLastUpdateUnix { get; init; }

    [JsonPropertyName("time_next_update_unix")]
    public long TimeNextUpdateUnix { get; init; }

    [JsonPropertyName("base_code")]
    public string? BaseCode { get; init; }

    [JsonPropertyName("conversion_rates")]
    public Dictionary<string, decimal>? ConversionRates { get; init; }
}