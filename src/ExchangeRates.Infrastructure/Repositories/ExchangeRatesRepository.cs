using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using ExchangeRates.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ExchangeRates.Infrastructure.Repositories;

internal class ExchangeRatesRepository(ApplicationDbContext dbContext) : IExchangeRatesRepository
{
    public async Task<int> Create(ExchangeRate entity)
    {
        dbContext.ExchangeRates.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<ExchangeRate?> GetByIdAsync(int id)
    {
        var exchangeRate = await dbContext.ExchangeRates
            .FirstOrDefaultAsync(x => x.Id == id);
        return exchangeRate;
    }

    public async Task Delete(ExchangeRate entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<ExchangeRate?> GetExchangeRateAsync(string fromCurrency, string toCurrency, DateTime date)
    {
        var exchangeRate = await dbContext.ExchangeRates
            .Where(x => x.FromCurrency == fromCurrency && x.ToCurrency == toCurrency && x.Date == date)
            .FirstOrDefaultAsync();

        return exchangeRate;
    }

    public async Task<(IEnumerable<ExchangeRate>, int)> GetAllMatchingAsync(string? searchPhrase,
    int pageSize, int pageNumber, string? sortBy, SortDirection sortDirection)
    {
        var searchPhraseUpper = searchPhrase?.ToUpper();

        var baseQuery = dbContext.ExchangeRates
            .Where(r => searchPhraseUpper == null ||
                (r.FromCurrency.ToUpper().Contains(searchPhraseUpper) || r.ToCurrency.ToUpper().Contains(searchPhraseUpper)));

        var totalCount = await baseQuery.CountAsync();

        if (sortBy != null)
        {
            var columnsSelector = new Dictionary<string, Expression<Func<ExchangeRate, object>>>
            {
                { nameof(ExchangeRate.FromCurrency), r => r.FromCurrency },
                { nameof(ExchangeRate.ToCurrency), r => r.ToCurrency },
                { nameof(ExchangeRate.Date), r => r.Date }
            };

            var selectedColumn = columnsSelector[sortBy];

            baseQuery = sortDirection == SortDirection.Ascending
                ? baseQuery.OrderBy(selectedColumn)
                : baseQuery.OrderByDescending(selectedColumn);
        }

        var exchangeRates = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (exchangeRates, totalCount);
    }

    public Task SaveChanges()
        => dbContext.SaveChangesAsync();
}
