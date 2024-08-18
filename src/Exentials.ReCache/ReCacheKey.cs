namespace Exentials.ReCache;

public sealed class ReCacheKey(string key, string nameSpace, KeyType keyType) : IReCacheKey, IEquatable<IReCacheKey>
{
    public static readonly string DefaultNamespace = string.Empty;

    public string Namespace { get; private set; } = nameSpace;
    public string Key { get; private set; } = key;
    public KeyType KeyType { get; private set; } = keyType;

    public bool Equals(IReCacheKey? other)
        => other is not null
           && Equals(Namespace, other.Namespace)
           && Equals(Key, other.Key)
           && Equals(KeyType, other.KeyType);

    public override bool Equals(object? obj) => obj is IReCacheKey namespaceKey && Equals(namespaceKey);

    public override int GetHashCode() => HashCode.Combine(Namespace, Key, KeyType);
    public override string ToString()
    {
        return $"{Namespace}:{Key}:{KeyType}";
    }

}
