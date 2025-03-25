using AutoMapper;
using ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;
using ExchangeRates.Application.ExchangeRates.Dtos;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Exceptions;
using ExchangeRates.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRate;

public class GetExchangeRateQueryHandler(
    ILogger<GetExchangeRateQueryHandler> logger,
    IMapper mapper,
    IExchangeRatesRepository exchangeRatesRepository,
    IExternalExchangeRatesRepository externalExchangeRatesRepository,
    IMediator mediator
    ) : IRequestHandler<GetExchangeRateQuery, ExchangeRateDto>
{
    public async Task<ExchangeRateDto> Handle(GetExchangeRateQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting Exchange Rate {FromCurrency} {ToCurrency} {Date}", request.FromCurrency, request.ToCurrency, request.Date);

        // If date is not provided, use the current date
        var rateDate = (request.Date ?? DateTime.UtcNow).Date;
        var fromCurrency = request.FromCurrency.ToUpper();
        var toCurrency = request.ToCurrency.ToUpper();

        // first get from Database
        var exchangeRate = await exchangeRatesRepository.GetExchangeRateAsync(fromCurrency, toCurrency, rateDate);

        if (exchangeRate is not null)
            return mapper.Map<ExchangeRateDto>(exchangeRate);

        if (exchangeRate is null && request.Date is null)
        {
            // get from external API
            exchangeRate = await externalExchangeRatesRepository.GetExchangeRateAsync(fromCurrency, toCurrency)
                ?? throw new NotFoundException(nameof(ExchangeRate), $"{request.FromCurrency}/{request.ToCurrency}");

            // save to local DB
            var createCommand = new CreateExchangeRateCommand
            {
                AskPrice = exchangeRate.AskPrice,
                BidPrice = exchangeRate.BidPrice,
                Date = exchangeRate.Date,
                FromCurrency = exchangeRate.FromCurrency,
                ToCurrency = exchangeRate.ToCurrency
            };
            await mediator.Send(createCommand);

            return mapper.Map<ExchangeRateDto>(exchangeRate);
        }
        else
            throw new NotFoundException(nameof(ExchangeRate), $"{request.FromCurrency}/{request.ToCurrency}/{rateDate.ToString("yyyy-MM-dd")}");

    }
}
