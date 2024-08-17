using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class DelHashSetCommand : ReCacheCommandBase
{
    private readonly KeyArgument keyArg = new();
    private readonly ValueArgument valueArg = new();
    private readonly NameSpaceOption namespaceOption = new();

    public DelHashSetCommand(ReCacheConnection connection)
        : base(connection, "delhashset")
    {
        AddArgument(keyArg);
        valueArg.SetDefaultValue(null);
        AddArgument(valueArg);
        AddOption(namespaceOption);

    }

    protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
    {
        var key = parameters.GetValueForArgument(keyArg);
        var value = parameters.GetValueForArgument(valueArg);
        var nameSpace = parameters.GetValueForOption(namespaceOption);

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
