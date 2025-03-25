namespace ExchangeRates.Domain.Entities;

public class ExchangeRate
{
    public int Id { get; set; }
    public string FromCurrency { get; set; } = default!;
    public string ToCurrency { get; set; } = default!;
    public decimal BidPrice { get; set; } // The price at which buyers are willing to purchase the currency
    public decimal AskPrice { get; set; } // The price at which sellers are willing to sell the currency
    public DateTime Date { get; set; } // Date of the exchange rate
    public DateTime CreatedAt { get; set; } // Timestamp of creation
    public DateTime? UpdatedAt { get; set; } // Timestamp of update
    public string? CreatedBy { get; set; } // User who created the record
}
