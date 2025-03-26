using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace ExchangeRates.Infrastructure.Integrations;

internal class AlphaVantageClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private const string BaseUrl = "https://www.alphavantage.co/query";

    public AlphaVantageClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["AlphaVantageApi:ApiKey"] 
            ?? throw new ArgumentNullException("AlphaVantage API Key is missing.");
    }

    public async Task<AlphaVantageResponse?> GetExchangeRateAsync(string fromCurrency, string toCurrency)
    {
        var url = $"{BaseUrl}?function=CURRENCY_EXCHANGE_RATE&from_currency={fromCurrency}&to_currency={toCurrency}&apikey={_apiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode(); // Throws exception if request fails

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<AlphaVantageResponse>(content);
    }
}
