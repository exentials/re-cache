namespace Exentials.ReCache
{
	internal sealed class ReCacheEntryHashSet : ReCacheEntry
	{
		public ReCacheEntryHashSet(object? value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
		{
			Value = value;
			AbsoluteExpiration = absoluteExpiration;
			SlidingExpiration = slidingExpiration;
		}
	}
}
