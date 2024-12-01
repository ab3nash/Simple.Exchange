using System.Net;

namespace Simple.Exchange.Domain.Exceptions;
public class ExchangeRateServiceException : Exception
{
    public HttpStatusCode StatusCode { get; }
    public string? BaseUrl { get; }
    public string? Endpoint { get; }
    public string? ResponseContent { get; }

    public ExchangeRateServiceException()
    {
    }

    public ExchangeRateServiceException(string message)
        : base(message)
    {
    }

    public ExchangeRateServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public ExchangeRateServiceException(
        HttpStatusCode statusCode, string baseUrl, string endpoint, string? responseContent = null)
        : base($"Request to {baseUrl}/{endpoint} failed with status code {statusCode}.")
    {
        StatusCode = statusCode;
        BaseUrl = baseUrl;
        Endpoint = endpoint;
        ResponseContent = responseContent;
    }

    public override string ToString()
    {
        return $"{base.ToString()}, StatusCode: {StatusCode}, ApiUrl: {BaseUrl}/{Endpoint}, ResponseContent: {ResponseContent}";
    }
}