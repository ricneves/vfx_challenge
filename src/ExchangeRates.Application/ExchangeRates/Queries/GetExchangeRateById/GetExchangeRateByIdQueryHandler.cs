using AutoMapper;
using ExchangeRates.Application.ExchangeRates.Dtos;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Exceptions;
using ExchangeRates.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRateById;

public class GetExchangeRateByIdQueryHandler(ILogger<GetExchangeRateByIdQueryHandler> logger, IMapper mapper,
    IExchangeRatesRepository exchangeRatesRepository) : IRequestHandler<GetExchangeRateByIdQuery, ExchangeRateDto>
{
    public async Task<ExchangeRateDto> Handle(GetExchangeRateByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Exchange Rate {ExchangeRateId}", request.Id);
        var exchangeRate = await exchangeRatesRepository.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(ExchangeRate), request.Id.ToString());

        var exchangeRateDto = mapper.Map<ExchangeRateDto>(exchangeRate);

        return exchangeRateDto;
    }
}
