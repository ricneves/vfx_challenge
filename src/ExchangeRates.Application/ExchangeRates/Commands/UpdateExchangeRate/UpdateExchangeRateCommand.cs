using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate;

public class UpdateExchangeRateCommand : IRequest
{
    public int Id { get; set; }
    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }
}
