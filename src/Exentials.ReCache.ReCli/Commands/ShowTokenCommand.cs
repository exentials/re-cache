using Exentials.ReCache.Client;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands;

internal class ShowTokenCommand(ReCacheConnection connection) : ReCacheCommandBase(connection, "token")
{
    protected override Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
    {
        Console.WriteLine(client.AuthenticationToken);
        return Task.CompletedTask;
    }

}
