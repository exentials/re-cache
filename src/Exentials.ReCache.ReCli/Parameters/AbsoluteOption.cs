using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters;

internal class AbsoluteOption : Option<DateTime?>
{
	public AbsoluteOption()
		: base("--absolute")
	{
		Description = "The absolute expiration time for the cache entry.";
		Aliases.Add("-ax");
	}
}
