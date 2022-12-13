namespace Exentials.ReCache
{
	/// <summary>
	/// Represents a combination of key/value element to store 
	/// </summary>
	public interface IReCacheEntry
	{
		/// <summary>
		/// Represents the value of the entry
		/// </summary>
		public object? Value { get; }
		/// <summary>
		/// Represents the absolute expiration date/time for the entry
		/// </summary>
		public DateTimeOffset? AbsoluteExpiration { get; }
		/// <summary>
		/// Represents the sliding expiration time for the entry
		/// </summary>
		public TimeSpan? SlidingExpiration { get; }
	}
}
