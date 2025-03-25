using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;

namespace ExchangeRates.Domain.Repositories;

public interface IExchangeRatesRepository
{
    Task<int> Create(ExchangeRate entity);
    Task<ExchangeRate?> GetByIdAsync(int id);
    Task Delete(ExchangeRate entity);
    Task SaveChanges();
    Task<ExchangeRate?> GetExchangeRateAsync(string fromCurrency, string toCurrency, DateTime date);
    Task<(IEnumerable<ExchangeRate>, int)> GetAllMatchingAsync(string? searchPhrase, int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection);
}
