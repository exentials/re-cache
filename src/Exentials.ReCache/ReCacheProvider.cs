using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Exentials.ReCache;

/// <summary>
/// InMemory cache provider
/// </summary>
public sealed class ReCacheProvider
{
    private readonly ConcurrentDictionary<ReCacheKey, ReCacheEntry> _concurrentDictionary = new();
    private readonly JsonSerializerOptions _jsonDeserializeOptions = new() { Converters = { new ReCacheConcurrentDictionaryConverter() } };
    private readonly JsonSerializerOptions _jsonSerializeOptions = new()
    {
        IgnoreReadOnlyProperties = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new ReCacheConcurrentDictionaryConverter() },
    };


    /// <summary>
    /// Set an entry in memory cache
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">Key of the entry</param>
    /// <param name="value">The value to store in memory cache</param>
    /// <param name="absoluteExpiration">The absolute date/time when the entry will expire</param>
    /// <param name="slidingExpiration">The sliding time when the entry will expire</param>
    /// <returns>The cached entry</returns>
    public IReCacheEntry Set<T>(ReCacheKey key, T value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        ReCacheEntry newEntry = new()
        {
            Value = value,
            AbsoluteExpiration = absoluteExpiration,
            SlidingExpiration = slidingExpiration
        };
        _concurrentDictionary.AddOrUpdate(
            key, newEntry,
            (key, entry) => newEntry    // discard the old entry
        );
        return newEntry;
    }

    /// <summary>
    /// Get a cached entry
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="key">Key of the entry</param>
    /// <returns>The cached value or null if it does not exist</returns>
    public T? Get<T>(ReCacheKey key)
    {
        if (_concurrentDictionary.TryGetValue(key, out ReCacheEntry? entry))
        {
            return (T?)entry.Value;
        }
        return default;
    }

    /// <summary>
    /// Delete a cached entry
    /// </summary>
    /// <param name="key">Key of the entry</param>
    /// <returns>The deleted cached value or null if it does not exist</returns>
    public T? Del<T>(ReCacheKey key)
    {
        if (_concurrentDictionary.TryRemove(key, out ReCacheEntry? entry))
        {
            return (T?)entry.Value;
        }
        return default;
    }

    /// <summary>
    /// Set an hashset entry in memory cache
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="key">Key of the entry</param>
    /// <param name="value">The value to store in the hashset memory cache</param>
    /// <param name="absoluteExpiration">The absolute date/time when the entry will expire</param>
    /// <param name="slidingExpiration">The sliding time when the entry will expire</param>
    public void SetHashSet<T>(ReCacheKey key, T value, DateTimeOffset? absoluteExpiration = null, TimeSpan? slidingExpiration = null)
    {
        var entryValue = Get<HashSet<T>>(key);
        if (entryValue is null)
        {
            entryValue = new HashSet<T> { value };
            Set(key, entryValue, absoluteExpiration, slidingExpiration);
        }
        else
        {
            entryValue.Add(value);
        }
    }

    /// <summary>
    /// Get a cached hashset entry
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    /// <param name="key">Key of the entry</param>
    /// <returns>The cached hashset values or null if it does not exist</returns>
    public HashSet<T>? GetHashSet<T>(ReCacheKey key)
    {
        return Get<HashSet<T>>(key);
    }

    /// <summary>
    /// Remove from the HashSet table a value
    /// </summary>
    /// <typeparam name="T">Type of the value to remove</typeparam>
    /// <param name="key">NamespaceKey of the HashSet table</param>
    /// <param name="value">Value to remove</param>
    /// <returns>The value removed or null if does not exist</returns>
    public T? DelHashSet<T>(ReCacheKey key, T value)
    {
        var entryValue = Get<HashSet<T>>(key);
        if (entryValue is null)
        {
            return default;
        }
        else
        {
            entryValue.RemoveWhere(x => Equals(x, value));
            return value;
        }
    }

    /// <summary>
    /// Remove an entry from the cache memory
    /// </summary>
    /// <param name="key">NamespaceKey of the entry</param>
    /// <returns>The entry removed or null if does not exist</returns>
    public IReCacheEntry? RemoveEntry(ReCacheKey key)
    {
        return _concurrentDictionary.Remove(key, out ReCacheEntry? entry)
            ? entry
            : null;
    }

    /// <summary>
    /// Retrieve the list of keys in memory by namespace
    /// </summary>
    /// <param name="nameSpace">Namespace to retrieve</param>
    /// <returns></returns>
    public IEnumerable<IReCacheKey> Keys(string nameSpace)
    {
        return KeysInternal(nameSpace);
    }

    private IEnumerable<IReCacheKey> KeysInternal(string? nameSpace = null)
    {
        return _concurrentDictionary.Keys.Where(x => nameSpace == null || x.Namespace == nameSpace);
    }

    /// <summary>
    /// Retrieve a list of namespaces
    /// </summary>
    /// <param name="keyType">The type of key</param>
    /// <returns>An <href /> </returns>
    public IEnumerable<string> Namespaces(KeyType keyType)
    {
        return _concurrentDictionary.Keys.Where(t => t.KeyType == keyType).GroupBy(t => t.Namespace).Select(t => t.Key);
    }

    /// <summary>
    /// Clear the memory cache by namespace
    /// </summary>
    /// <param name="nameSpace"></param>
    public void Clear(string nameSpace)
    {
        foreach (var entry in _concurrentDictionary.Where(t => t.Key.Namespace == nameSpace))
        {
            _concurrentDictionary.TryRemove(entry);
        }
    }

    /// <summary>
    /// Clear the memory cache
    /// </summary>
    public void ClearAll()
    {
        _concurrentDictionary.Clear();
    }

    /// <summary>
    /// Clean expired memory cache entries
    /// </summary>
    /// <returns>Number of entries removed</returns>
    public int CleanExpired()
    {
        int i = 0;
        foreach (var entry in _concurrentDictionary.Where(t => t.Value.Expired))
        {
            _concurrentDictionary.TryRemove(entry);
            i++;
        }
        return i;
    }

    /// <summary>
    /// Deserialize cache data from a stream
    /// </summary>
    /// <param name="stream"></param>
    public void DeserializeFrom(Stream stream)
    {
        var items = JsonSerializer.Deserialize<ConcurrentDictionary<ReCacheKey, ReCacheEntry>>(stream, _jsonDeserializeOptions);
        if (items is not null)
        {
            _concurrentDictionary.Clear();
            foreach (var i in items)
            {
                _concurrentDictionary.TryAdd(i.Key, i.Value);
            }
        }
    }

    /// <summary>
    /// Serialize cache data to a stream
    /// </summary>
    /// <param name="stream"></param>
    public void SerializeTo(Stream stream)
    {
        JsonSerializer.Serialize(stream, _concurrentDictionary, _jsonSerializeOptions);
    }

}
