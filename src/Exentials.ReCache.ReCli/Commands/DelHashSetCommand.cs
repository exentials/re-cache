using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class DelHashSetCommand : ReCacheCommandBase
{
	private readonly KeyArgument keyArg = new();
	private readonly ValueArgument valueArg = new();
	private readonly NameSpaceOption namespaceOption = new();

	public DelHashSetCommand(ReCacheConnection connection)
		: base(connection, "delhashset")
	{
		Arguments.Add(keyArg);
		Arguments.Add(valueArg);
		Options.Add(namespaceOption);

	}

	protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		var key = parameters.GetValue(keyArg);
		var value = parameters.GetValue(valueArg);
		var nameSpace = parameters.GetValue(namespaceOption);

		if (value is null)
		{
			await client.RemoveHashSetAsync(key, nameSpace);
		}
		else
		{
			await client.DelHashSetAsync(key, value, nameSpace);
		}
	}
}
