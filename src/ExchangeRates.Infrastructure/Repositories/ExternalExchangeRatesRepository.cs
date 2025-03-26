using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using ExchangeRates.Infrastructure.Integrations;

namespace ExchangeRates.Infrastructure.Repositories;

internal class ExternalExchangeRatesRepository(AlphaVantageClient alphaVantageClient) : IExternalExchangeRatesRepository
{
    public async Task<ExchangeRate?> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        var exchangeRateExternal = await alphaVantageClient.GetExchangeRateAsync(fromCurrency, toCurrency);

        return new ExchangeRate
        {
            AskPrice = exchangeRateExternal.ExchangeRate.AskPrice,
            BidPrice = exchangeRateExternal.ExchangeRate.BidPrice,
            FromCurrency = exchangeRateExternal.ExchangeRate.FromCurrency,
            ToCurrency = exchangeRateExternal.ExchangeRate.ToCurrency,
            Date = exchangeRateExternal.ExchangeRate.Date.Date
        };
    }
}
