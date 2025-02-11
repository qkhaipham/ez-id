using System.Text.Json;
using FluentAssertions;
using QKP.EzId.Json;

namespace QKP.EzId.Tests.Json;

public class EzIdJsonConverterTests
{
    private readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new EzIdJsonConverter() }
    };

    [Fact]
    public void Given_json_with_id_as_string_when_deserializing_then_it_must_deserialize_as_expected()
    {
        var json = @"{""id"":""07047XF6Q8YPA"",""name"":""John Doe""}";

        // Act
        var result = JsonSerializer.Deserialize<Person>(json, _options);

        // Assert
        result.Should().NotBeNull();
        result!.Id.ToString().Should().Be("07047XF6Q8YPA");
    }

    [Fact]
    public void Given_ez_id_when_serializing_then_it_must_serialize_as_expected()
    {
        EzId id = EzId.Parse("07047XF6Q8YPA");
        var person = new Person(id, "John Doe");
        string expectedJson = @"{""id"":""07047XF6Q8YPA"",""name"":""John Doe""}";

        // Act
        string result = JsonSerializer.Serialize(person, _options);

        // Assert
        result.Should().Be(expectedJson);
    }
}

public record Person(EzId Id, string Name);
