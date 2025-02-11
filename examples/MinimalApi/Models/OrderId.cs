using System.Text.Json;
using System.Text.Json.Serialization;
using QKP.EzId;

namespace MinimalApi.Models;

// if you want to create your own derived types of EzId.
[JsonConverter(typeof(OrderIdJsonConverter))]
public class OrderId(long value) : EzId(value), IParsable<OrderId>
{
    public static new OrderId Parse(string s, IFormatProvider? provider)
    {
        var id = Parse(s);
        return new OrderId(id.Value);
    }

    public static bool TryParse(string? s, IFormatProvider? provider, out OrderId result)
    {
        bool success = EzId.TryParse(s, provider, out EzId ezId);
        result = new OrderId(ezId.Value);
        return success;
    }
}

public class OrderIdJsonConverter : JsonConverter<OrderId>
{
    /// <inheritdoc />
    public override OrderId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException("Expected a string.");
        }

        return OrderId.Parse(reader.GetString() ?? string.Empty, null);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, OrderId value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.StringValue);
    }
}
