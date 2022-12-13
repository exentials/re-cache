using System.Text.Json;
using System.Text.Json.Serialization;

namespace Exentials.ReCache
{
	internal sealed class ReCacheKeyConverter : JsonConverter<ReCacheKey>
	{
		public override ReCacheKey? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if (reader.TokenType != JsonTokenType.StartObject)
			{
				throw new JsonException();
			}
			reader.Read();

			string? propertyName = reader.GetString();
			if (!Equals(propertyName, nameof(ReCacheKey.Key)))
			{
				throw new NotImplementedException();
			}
			reader.Read();
			string? key = reader.GetString();

			reader.Read();
			propertyName = reader.GetString();
			if (!Equals(propertyName, nameof(ReCacheKey.Namespace)))
			{
				throw new JsonException();
			}
			reader.Read();
			string? nameSpace = reader.GetString();

			reader.Read();
			propertyName = reader.GetString();
			if (!Equals(propertyName, nameof(ReCacheKey.KeyType)))
			{
				throw new JsonException();
			}
			reader.Read();
			if (!Enum.TryParse<KeyType>(reader.GetString(), true, out KeyType keyType))
			{
				throw new JsonException();
			}

			reader.Read();
			if (reader.TokenType != JsonTokenType.EndObject)
			{
				throw new JsonException();
			}

			reader.Read();

			return (key is null || nameSpace is null)
				? null
				: new ReCacheKey(key, nameSpace, keyType);
		}

		public override void Write(Utf8JsonWriter writer, ReCacheKey value, JsonSerializerOptions options)
		{
			writer.WriteStartObject();

			writer.WritePropertyName(nameof(ReCacheKey.Key));
			writer.WriteStringValue(value.Key);

			writer.WritePropertyName(nameof(ReCacheKey.Namespace));
			writer.WriteStringValue(value.Namespace);

			writer.WritePropertyName(nameof(ReCacheKey.KeyType));
			writer.WriteStringValue(value.KeyType.ToString());

			writer.WriteEndObject();
		}

	}
}
