using System.Text.Json.Serialization;

namespace ExchangeRates.Infrastructure.Integrations;

internal class ExchangeRateData
{
    [JsonPropertyName("1. From_Currency Code")]
    public string FromCurrency { get; set; }

    [JsonPropertyName("3. To_Currency Code")]
    public string ToCurrency { get; set; }

    [JsonPropertyName("8. Bid Price")]
    [JsonConverter(typeof(CustomDecimalConverter))]
    public decimal BidPrice { get; set; }

    [JsonPropertyName("9. Ask Price")]
    [JsonConverter(typeof(CustomDecimalConverter))]
    public decimal AskPrice { get; set; }

    [JsonPropertyName("6. Last Refreshed")]
    [JsonConverter(typeof(CustomDateTimeConverter))]
    public DateTime Date { get; set; }
}

internal class AlphaVantageResponse
{
    [JsonPropertyName("Realtime Currency Exchange Rate")]
    public ExchangeRateData ExchangeRate { get; set; }
}

