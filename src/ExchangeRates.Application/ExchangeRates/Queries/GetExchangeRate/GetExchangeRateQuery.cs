using ExchangeRates.Application.ExchangeRates.Dtos;
using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRate;

public class GetExchangeRateQuery(string fromCurrency, string toCurrency, DateTime? date) : IRequest<ExchangeRateDto>
{
    public string FromCurrency { get; } = fromCurrency;
    public string ToCurrency { get; } = toCurrency;
    public DateTime? Date { get; } = date;
}
