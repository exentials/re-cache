using System.Collections.Concurrent;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Exentials.ReCache;

internal sealed partial class ReCacheConcurrentDictionaryConverter : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert == typeof(ConcurrentDictionary<ReCacheKey, ReCacheEntry>)
            || typeToConvert == typeof(ReCacheKey)
            || typeToConvert == typeof(ReCacheEntrySet)
            || typeToConvert == typeof(ReCacheEntryHashSet)
            || typeToConvert == typeof(ReCacheEntry);
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        JsonConverter? converter;
        if (typeToConvert == typeof(ConcurrentDictionary<ReCacheKey, ReCacheEntry>))
        {
            converter = new ReCacheDictionaryConverter();
        }
        else if (typeToConvert == typeof(ReCacheKey))
        {
            converter = new ReCacheKeyConverter();
        }
        else if (
            typeToConvert == typeof(ReCacheEntrySet)
            || typeToConvert == typeof(ReCacheEntryHashSet)
            || typeToConvert == typeof(ReCacheEntry))
        {
            converter = new ReCacheEntryConverter();
        }
        else
        {
            converter = null;
        }
        return converter;
    }
}