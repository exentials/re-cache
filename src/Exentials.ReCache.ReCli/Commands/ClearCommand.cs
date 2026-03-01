using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal class ClearCommand : ReCacheCommandBase
{
	private readonly NameSpaceOption namespaceOption = new();

	public ClearCommand(ReCacheConnection connection)
		: base(connection, "clear", "Clear cache")
	{
		Options.Add(namespaceOption);
	}

	protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		var nameSpace = parameters.GetValue(namespaceOption);
		await client.Clear(nameSpace);
	}
}
