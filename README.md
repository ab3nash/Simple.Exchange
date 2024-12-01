# Simple Exchange Service API

## How to Run
1. Clone the repository and open folder Simple.Exchange.Api
2. Replace `ExchangeRateApiConfig:ApiKey` with your Free API key from [ExchangeRate-API](https://www.exchangerate-api.com/docs/free) in appsettings.json.
3. Restore dependencies: `dotnet restore`.
4. Run the application: `dotnet run`.
5. Test the API using the example request below.

## Example Request
```
curl -k -X POST   https://localhost:7107/ExchangeService   -H "Content-Type: application/json"   -d '{
    "amount": 100,
    "inputCurrency": "AUD",
    "outputCurrency": "USD"
  }'
 
```

## Caveats
- Only supports AUD to USD conversion.
- No authentication or rate limiting implemented.
- Not production-ready (detailed logging and better exception handling could be implemented).
- Test coverage is minimal and only for demonstration
- Since a memory cache is used and it's invalidation is determined by the API response, a secondary invalidaiton approach may be required to invalidate cache without restarting the applicaiton
- Added retry logic for sake of completeness, but needs refinement
- Failure responses can be improved
- Self-signed developer cert is used, so ssl certifcate needs to be ignored in curl request `-k`