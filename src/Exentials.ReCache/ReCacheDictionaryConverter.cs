using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Exentials.ReCache
{
	public sealed class ReCacheDictionaryConverter : JsonConverter<ConcurrentDictionary<ReCacheKey, ReCacheEntry>>
	{
		const string KEY = "key";
		const string ENTRY = "entry";

		public override ConcurrentDictionary<ReCacheKey, ReCacheEntry>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartArray)
			{
				throw new JsonException();
			}
			var dictionary = new ConcurrentDictionary<ReCacheKey, ReCacheEntry>();
			while (reader.Read())
			{
				if (reader.TokenType == JsonTokenType.EndArray)
				{
					return dictionary;
				}
				else
				{
					if (reader.TokenType != JsonTokenType.StartObject)
					{
						throw new JsonException();
					}
					reader.Read();

					// key
					string? propertyName = reader.GetString();
					if (!Equals(propertyName, KEY))
					{
						throw new JsonException();
					}
					var key = JsonSerializer.Deserialize<ReCacheKey>(ref reader, options);
					if (reader.TokenType != JsonTokenType.EndObject)
					{
						throw new JsonException();
					}
					reader.Read();

					// value
					propertyName = reader.GetString();
					if (!Equals(propertyName, ENTRY))
					{
						throw new JsonException();
					}

					ReCacheEntry? value;
					if (key is null)
					{
						throw new JsonException();
					}
					if (key.KeyType == KeyType.Set)
					{
						value = JsonSerializer.Deserialize<ReCacheEntrySet>(ref reader, options) as ReCacheEntry;
					}
					else if (key.KeyType == KeyType.HashSet)
					{
						value = JsonSerializer.Deserialize<ReCacheEntryHashSet>(ref reader, options) as ReCacheEntry;
					}
					else
					{
						throw new JsonException();
					}

					reader.Read();
					if (reader.TokenType != JsonTokenType.EndObject)
					{
						throw new JsonException();
					}

					if (key is null || value is null)
					{
						throw new JsonException();
					}
					dictionary.TryAdd(key, value);
				}
			}

			throw new JsonException();
		}

		public override void Write(Utf8JsonWriter writer, ConcurrentDictionary<ReCacheKey, ReCacheEntry> value, JsonSerializerOptions options)
		{

			writer.WriteStartArray();
			foreach (var i in value.ToImmutableArray())
			{
				writer.WriteStartObject();

				writer.WritePropertyName(KEY);
				var jsonKey = JsonSerializer.Serialize(i.Key, options);
				writer.WriteRawValue(jsonKey);

				writer.WritePropertyName(ENTRY);
				var jsonValue = JsonSerializer.Serialize(i.Value, options);
				writer.WriteRawValue(jsonValue);

				writer.WriteEndObject();
			}
			writer.WriteEndArray();
		}
	}
}