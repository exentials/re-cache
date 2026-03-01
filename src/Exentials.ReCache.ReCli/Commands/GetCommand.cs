using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class GetCommand : ReCacheCommandBase
{
	private readonly KeyArgument keyArg = new();
	private readonly NameSpaceOption namespaceOption = new();

	public GetCommand(ReCacheConnection connection)
		: base(connection, "get")
	{
		Arguments.Add(keyArg);
		Options.Add(namespaceOption);
	}

	protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		var key = parameters.GetValue(keyArg);
		var nameSpace = parameters.GetValue(namespaceOption);

		var value = await client.GetAsync(key, nameSpace);
		Console.WriteLine($"{value}");
	}
}
