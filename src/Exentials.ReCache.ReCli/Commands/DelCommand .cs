using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class DelCommand : ReCacheCommandBase
{
	private readonly KeyArgument keyArg = new();
	private readonly NameSpaceOption namespaceOption = new();

	public DelCommand(ReCacheConnection connection)
		: base(connection, "del")
	{
		Arguments.Add(keyArg);
		Options.Add(namespaceOption);
	}

	protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		var key = parameters.GetValue(keyArg);
		var nameSpace = parameters.GetValue(namespaceOption);

		await client.DelAsync(key, nameSpace);
	}

}
