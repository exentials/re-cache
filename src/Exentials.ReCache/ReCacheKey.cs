namespace Exentials.ReCache
{
	public sealed class ReCacheKey : IReCacheKey, IEquatable<IReCacheKey>
	{
		public static readonly string DefaultNamespace = string.Empty;

		public ReCacheKey(string key, string nameSpace, KeyType keyType)
		{
			Key = key;
			Namespace = nameSpace;
			KeyType = keyType;
		}

		public string Namespace { get; private set; }
		public string Key { get; private set; }
		public KeyType KeyType { get; private set; }

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
}
