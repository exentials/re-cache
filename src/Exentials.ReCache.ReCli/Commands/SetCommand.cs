using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class SetCommand : ReCacheCommandBase
{
	private readonly KeyArgument keyArg = new();
	private readonly ValueArgument valueArg = new();
	private readonly NameSpaceOption namespaceOption = new();
	private readonly AbsoluteOption absoluteOption = new();
	private readonly SlidingOption slidingOption = new();

	public SetCommand(ReCacheConnection connection)
		: base(connection, "set")
	{
		Arguments.Add(keyArg);
		Arguments.Add(valueArg);
		Options.Add(namespaceOption);
		Options.Add(absoluteOption);
		Options.Add(slidingOption);
	}

	protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		var key = parameters.GetValue(keyArg);
		var value = parameters.GetValue(valueArg);
		var absolute = parameters.GetValue(absoluteOption);
		var sliding = parameters.GetValue(slidingOption);

		var nameSpace = parameters.GetValue(namespaceOption);

		if (await client.SetAsync(key, value, absolute, sliding, nameSpace))
		{
			Console.WriteLine($"{value} cached");
		}
	}

}
