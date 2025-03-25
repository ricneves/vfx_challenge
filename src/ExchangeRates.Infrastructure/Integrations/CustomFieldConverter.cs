using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExchangeRates.Infrastructure.Integrations;

internal class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private const string DateFormat = "yyyy-MM-dd HH:mm:ss"; // Match API format

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateTime.TryParseExact(reader.GetString(), DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            return date;
        }
        throw new JsonException($"Invalid date format. Expected {DateFormat}");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(DateFormat, CultureInfo.InvariantCulture));
    }
}

internal class CustomDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (decimal.TryParse(reader.GetString(), NumberStyles.Any, CultureInfo.InvariantCulture, out var value))
        {
            return value;
        }
        throw new JsonException("Invalid decimal format.");
    }

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString(CultureInfo.InvariantCulture));
    }
}