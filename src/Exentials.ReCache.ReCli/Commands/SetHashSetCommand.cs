using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class SetHashSetCommand : ReCacheCommandBase
{
    private readonly KeyArgument keyArg = new();
    private readonly ValueArgument valueArg = new();
    private readonly NameSpaceOption namespaceOption = new();

    public SetHashSetCommand(ReCacheConnection connection)
        : base(connection, "sethashset")
    {
        AddArgument(keyArg);
        AddArgument(valueArg);
        AddOption(namespaceOption);
    }

    protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
    {
        var key = parameters.GetValueForArgument(keyArg);
        var value = parameters.GetValueForArgument(valueArg);

        var nameSpace = parameters.GetValueForOption(namespaceOption);

        if (await client.SetHashSetAsync(key, value, null, null, nameSpace))
        {
            Console.WriteLine($"{value} cached");
        }
    }
}
