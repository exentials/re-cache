using System.CommandLine;

namespace Exentials.ReCache.ReCli.Parameters;

internal class KeyArgument : Argument<string>
{
	public KeyArgument()
		: base("key")
	{
		Description = "The key to cache the value under.";
	}
}
