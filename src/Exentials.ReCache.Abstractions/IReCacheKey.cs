namespace Exentials.ReCache
{
	/// <summary>
	/// Define a combination of Namespace/Key to store an object in a dictionary
	/// </summary>
	public interface IReCacheKey : IEquatable<IReCacheKey>
	{
		/// <summary>
		/// Represent the namespace part for a group of keys
		/// </summary>
		public string Namespace { get; }
		/// <summary>
		/// Represent the key part
		/// </summary>
		public string Key { get; }
		/// <summary>
		/// Represent the type of the key 
		/// </summary>
		public KeyType KeyType { get; }
	}
}