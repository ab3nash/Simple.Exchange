namespace Simple.Exchange.Domain.Exceptions;

public class ExchangeServiceException : Exception
{
    public ExchangeServiceException()
    {
    }

    public ExchangeServiceException(string message)
        : base(message)
    {
    }

    public ExchangeServiceException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}