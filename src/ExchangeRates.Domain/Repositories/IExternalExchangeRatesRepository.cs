using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Domain.Repositories;

public interface IExternalExchangeRatesRepository
{
    Task<ExchangeRate?> GetExchangeRateAsync(string fromCurrency, string toCurrency);
}
