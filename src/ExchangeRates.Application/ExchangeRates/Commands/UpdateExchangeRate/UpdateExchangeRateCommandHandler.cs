using AutoMapper;
using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Exceptions;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate;

public class UpdateExchangeRateCommandHandler(ILogger<UpdateExchangeRateCommandHandler> logger, IExchangeRatesRepository exchangeRatesRepository,
    IMapper mapper, IApplicationAuthorizationService applicationAuthorizationService) : IRequestHandler<UpdateExchangeRateCommand>
{
    public async Task Handle(UpdateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating Exchange Rate with id : {ExchangeRateId} with {@UpdatedExchangeRate}", request.Id, request);
        var exchangeRate = await exchangeRatesRepository.GetByIdAsync(request.Id);
        if (exchangeRate is null)
            throw new NotFoundException(nameof(ExchangeRate), request.Id.ToString());

        if (!applicationAuthorizationService.Authorize(exchangeRate, ResourceOperation.Update))
            throw new ForbidException();

        mapper.Map(request, exchangeRate);

        exchangeRate.UpdatedAt = DateTime.UtcNow;

        await exchangeRatesRepository.SaveChanges();
    }
}
