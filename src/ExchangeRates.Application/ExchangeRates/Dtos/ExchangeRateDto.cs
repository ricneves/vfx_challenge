namespace ExchangeRates.Application.ExchangeRates.Dtos;

public class ExchangeRateDto
{
    public string FromCurrency { get; set; } = default!;
    public string ToCurrency { get; set; } = default!;
    public decimal BidPrice { get; set; } // The price at which buyers are willing to purchase the currency
    public decimal AskPrice { get; set; } // The price at which sellers are willing to sell the currency
    public DateTime Date { get; set; } // Date of the exchange rate
}
