using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters;

internal class SlidingOption : Option<TimeSpan?>
{
	public SlidingOption()
		: base("--sliding")
	{
		Description = "The sliding expiration time for the cache entry.";
		Aliases.Add("-sx");	
	}
}
