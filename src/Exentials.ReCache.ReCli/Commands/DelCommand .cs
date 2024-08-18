using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class DelCommand : ReCacheCommandBase
{
    private readonly KeyArgument keyArg = new();
    private readonly NameSpaceOption namespaceOption = new();

    public DelCommand(ReCacheConnection connection)
        : base(connection, "del")
    {
        AddArgument(keyArg);
        AddOption(namespaceOption);
    }

    protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
    {
        var key = parameters.GetValueForArgument(keyArg);
        var nameSpace = parameters.GetValueForOption(namespaceOption);

        await client.DelAsync(key, nameSpace);
    }

}
