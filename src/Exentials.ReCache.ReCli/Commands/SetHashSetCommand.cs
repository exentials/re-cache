using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class SetHashSetCommand : ReCacheCommandBase
{
	private readonly KeyArgument keyArg = new();
	private readonly ValueArgument valueArg = new();
	private readonly NameSpaceOption namespaceOption = new();

	public SetHashSetCommand(ReCacheConnection connection)
		: base(connection, "sethashset")
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

		if (await client.SetHashSetAsync(key, value, null, null, nameSpace))
		{
			Console.WriteLine($"{value} cached");
		}
	}
}
