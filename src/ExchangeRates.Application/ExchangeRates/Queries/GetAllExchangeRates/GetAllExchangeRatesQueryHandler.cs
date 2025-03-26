using AutoMapper;
using ExchangeRates.Application.Common;
using ExchangeRates.Application.ExchangeRates.Dtos;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.ExchangeRates.Queries.GellAllExchangeRates;

public class GetAllExchangeRatesQueryHandler(ILogger<GetAllExchangeRatesQueryHandler> logger, IMapper mapper,
    IExchangeRatesRepository exchangeRatesRepository) : IRequestHandler<GetAllExchangeRatesQuery, PagedResult<ExchangeRate>>
{
    public async Task<PagedResult<ExchangeRate>> Handle(GetAllExchangeRatesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all ExchangeRates");

        var (exchangeRates, totalCount) = await exchangeRatesRepository.GetAllMatchingAsync(
            request.SearchPhrase, request.PageSize, request.PageNumber, request.SortBy, request.SortDirection);

        //var exchangeRatesDtos = mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates);

        var result = new PagedResult<ExchangeRate>(exchangeRates, totalCount, request.PageSize, request.PageNumber);

        return result;
    }
}
