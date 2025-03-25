using ExchangeRates.Application.Common;
using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Queries.GellAllExchangeRates;

public class GetAllExchangeRatesQuery : IRequest<PagedResult<ExchangeRate>>
{
    public string? SearchPhrase { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public string? SortBy { get; set; }
    public SortDirection SortDirection { get; set; }
}
