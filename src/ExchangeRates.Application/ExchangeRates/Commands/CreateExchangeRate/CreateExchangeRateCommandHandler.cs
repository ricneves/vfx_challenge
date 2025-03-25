using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ExchangeRates.Application.Users;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Domain.Repositories;

namespace ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;

public class CreateExchangeRateCommandHandler(ILogger<CreateExchangeRateCommandHandler> logger, IMapper mapper,
    IExchangeRatesRepository exchangeRatesRepository, IUserContext userContext) : IRequestHandler<CreateExchangeRateCommand, int>
{
    public async Task<int> Handle(CreateExchangeRateCommand request, CancellationToken cancellationToken)
    {
        var currentUser = userContext.GetCurrentUser();

        logger.LogInformation("{UserEmail} [{UserId}] is creating a new Exchange Rate {@ExchangeRate}",
            currentUser?.Email, currentUser?.Id, request);

        var exchangeRate = mapper.Map<ExchangeRate>(request);
        exchangeRate.CreatedBy = currentUser?.Id;
        exchangeRate.CreatedAt = DateTime.UtcNow;

        int id = await exchangeRatesRepository.Create(exchangeRate);

        return id;

    }
}
