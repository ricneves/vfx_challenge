using MediatR;

namespace ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;

public class CreateExchangeRateCommand : IRequest<int>
{
    public string FromCurrency { get; set; } = default!;
    public string ToCurrency { get; set; } = default!;
    public decimal BidPrice { get; set; }
    public decimal AskPrice { get; set; }
    public DateTime Date { get; set; }
}
