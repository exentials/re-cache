using System.Text.Json;
using System.Text.Json.Serialization;

namespace Exentials.ReCache;

internal class ReCacheEntryConverter : JsonConverter<ReCacheEntry>
{
    private static object? ReadSet(ref Utf8JsonReader reader)
    {
        reader.Read();
        object? value;
        if (reader.TokenType == JsonTokenType.Number)
        {
            value = reader.GetInt64().ToString();
        }
        else if (reader.TokenType == JsonTokenType.String)
        {
            value = reader.GetString();
        }
        else
        {
            value = null;
        }
        return value;
    }

    private static object? ReadHashSet(ref Utf8JsonReader reader)
    {
        //reader.Read();
        object? value = JsonSerializer.Deserialize<HashSet<string>>(ref reader);
        return value;
    }

    public override ReCacheEntry? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException();
        }
        reader.Read();

        string? propertyName = reader.GetString();
        if (!Equals(propertyName, nameof(ReCacheEntry.Value)))
        {
            throw new NotImplementedException();
        }

        object? value;
        if (typeToConvert == typeof(ReCacheEntrySet))
        {
            value = ReadSet(ref reader);
        }
        else if (typeToConvert == typeof(ReCacheEntryHashSet))
        {
            value = ReadHashSet(ref reader);
        }
        else
        {
            throw new NotImplementedException();
        }

        reader.Read();

        DateTime? absoluteExpiration = null;
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            propertyName = reader.GetString();
            if (!Equals(propertyName, nameof(ReCacheEntry.AbsoluteExpiration)))
            {
                throw new NotImplementedException();
            }
            reader.Read();
            if (DateTime.TryParse(reader.GetString(), out DateTime dateTime))
            {
                absoluteExpiration = dateTime;
            }
        }

        TimeSpan? slidingExpiration = null;
        if (reader.TokenType != JsonTokenType.EndObject)
        {
            propertyName = reader.GetString();
            if (!Equals(propertyName, nameof(ReCacheEntry.SlidingExpiration)))
            {
                throw new NotImplementedException();
            }
            reader.Read();
            if (TimeSpan.TryParse(reader.GetString(), out TimeSpan timeSpan))
            {
                slidingExpiration = timeSpan;
            }
        }

        ReCacheEntry? entry = (ReCacheEntry?)Activator.CreateInstance(typeToConvert, [value, absoluteExpiration, slidingExpiration]);
        if (entry is null)
        {
            throw new NotImplementedException();
        }

        return entry;
    }

    public override void Write(Utf8JsonWriter writer, ReCacheEntry value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WritePropertyName(nameof(ReCacheEntry.Value));
        if (value.Value is not null)
        {
            if (value.Value.GetType() == typeof(string))
            {
                writer.WriteRawValue((string)value.Value);
            }
            else
            {
                var json = JsonSerializer.Serialize(value.Value);
                writer.WriteRawValue(json);
            }
        }

        if (value.AbsoluteExpiration is not null)
        {
            writer.WriteString(nameof(ReCacheEntry.AbsoluteExpiration), value.AbsoluteExpiration.Value);
        }

        if (value.SlidingExpiration is not null)
        {
            var time = value.SlidingExpiration.Value;
            writer.WriteNumber(nameof(ReCacheEntry.SlidingExpiration), time.TotalSeconds);
        }

        writer.WriteEndObject();
    }
}
