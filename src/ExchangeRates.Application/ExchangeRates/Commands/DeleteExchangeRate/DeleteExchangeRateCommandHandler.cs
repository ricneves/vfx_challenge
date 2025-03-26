using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Exceptions;
using ExchangeRates.Domain.Interfaces;
using ExchangeRates.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Application.ExchangeRates.Commands.DeleteExchangeRate;

public class DeleteExchangeRateCommandHandler(ILogger<DeleteExchangeRateCommandHandler> logger, IExchangeRatesRepository exchangeRatesRepository,
    IApplicationAuthorizationService applicationAuthorizationService) : IRequestHandler<DeleteExchangeRateCommand>
{
    public async Task Handle(DeleteExchangeRateCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Deleting ExchangeRate with id:{request.Id}");

        var exchangeRate = await exchangeRatesRepository.GetByIdAsync(request.Id);

        if (exchangeRate is null)
            throw new NotFoundException(nameof(ExchangeRate), request.Id.ToString());

        if (!applicationAuthorizationService.Authorize(exchangeRate, ResourceOperation.Delete))
            throw new ForbidException();

        await exchangeRatesRepository.Delete(exchangeRate);
    }
}
