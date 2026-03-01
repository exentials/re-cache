using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class GetHashSetCommand : ReCacheCommandBase
{
	private readonly KeyArgument keyArg = new();
	private readonly NameSpaceOption namespaceOption = new();

	public GetHashSetCommand(ReCacheConnection connection)
		: base(connection, "gethashset")
	{
		Arguments.Add(keyArg);
		Options.Add(namespaceOption);
	}

	protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		var key = parameters.GetValue(keyArg);
		var nameSpace = parameters.GetValue(namespaceOption);

		var hashSet = await client.GetHashSetAsync(key, nameSpace);
		Console.WriteLine($"HashSet values:");
		if (hashSet is not null)
		{
			foreach (var value in hashSet)
			{
				Console.WriteLine($"{value}");
			}
		}

	}
}
