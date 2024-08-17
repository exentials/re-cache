using Exentials.ReCache.Client;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands;

internal abstract class ReCacheCommandBase : Command
{
    protected readonly ReCacheConnection Connection;
    public ReCacheCommandBase(ReCacheConnection connection, string name, string? description = null) : base(name, description)
    {
        Connection = connection;
        this.SetHandler(CommandHandler);
    }

    protected virtual async Task CommandHandler(InvocationContext context)
    {
        if (Connection.IsConnected && Connection.Client is not null)
        {
            ParseResult parameters = context.ParseResult;
            var cancellationToken = context.GetCancellationToken();
            await Invoke(Connection.Client, parameters, cancellationToken);
        }
        else
        {
            Console.WriteLine("You must connect first.");
        }
    }

    protected virtual Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
