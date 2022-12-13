using System.Text.Json.Serialization;

namespace Exentials.ReCache
{
	/// <summary>
	/// Define an entry to be store in a dictionary
	/// </summary>

	[JsonConverter(typeof(ReCacheEntryConverter))]
	public class ReCacheEntry : IReCacheEntry
	{
		private DateTimeOffset? _absoluteExpiration;
		private TimeSpan? _slidingExpiration;
		private DateTimeOffset? _slidindExpirationFromNow;
		private object? _value;

		/// <summary>
		/// Create a new entry
		/// </summary>
		public ReCacheEntry()
		{
		}

		/// <summary>
		/// Entry value
		/// </summary>
		public required object? Value
		{
			get
			{
				if (Expired)
				{
					return null;
				}
				else
				{
					UpdateSlidingExpirationFromNow();
					return _value;
				}
			}
			set
			{
				_value = value;
			}
		}

		/// <summary>
		/// Get/Set absolute date time expiration of the value
		/// </summary>
		public DateTimeOffset? AbsoluteExpiration { get => _absoluteExpiration; set => _absoluteExpiration = value; }
		/// <summary>
		/// Get/Set sliding timespan expiration of the value
		/// </summary>
		public TimeSpan? SlidingExpiration { get => _slidingExpiration; set => _slidingExpiration = value; }

		/// <summary>
		/// Get the expire state of the value
		/// </summary>
		public bool Expired
		{
			get => _absoluteExpiration.HasValue && _absoluteExpiration.Value <= DateTimeOffset.Now
				|| _slidindExpirationFromNow.HasValue && _slidindExpirationFromNow.Value <= DateTimeOffset.Now;
		}

		private void UpdateSlidingExpirationFromNow()
		{
			if (_slidingExpiration.HasValue)
			{
				_slidindExpirationFromNow = DateTimeOffset.Now.Add(_slidingExpiration.Value);
			}
		}

		public override string ToString()
		{
			return Value?.ToString() ?? string.Empty;
		}
	}
}