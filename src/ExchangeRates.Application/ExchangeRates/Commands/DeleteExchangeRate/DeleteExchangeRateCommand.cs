using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Commands.DeleteExchangeRate;

public class DeleteExchangeRateCommand(int id) : IRequest
{
    public int Id { get; } = id;
}
