namespace Exentials.ReCache
{
	internal class ReCacheEntrySet : ReCacheEntry
	{
		public ReCacheEntrySet(object? value, DateTime? absoluteExpiration, TimeSpan? slidingExpiration)
		{
			Value = value;
			AbsoluteExpiration = absoluteExpiration;
			SlidingExpiration = slidingExpiration;
		}
	}
}
