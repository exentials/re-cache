using Exentials.ReCache.Client;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class ReCacheRootCommand : RootCommand
{
    private readonly ReCacheConnection _conn = new();
    public ReCacheRootCommand()
        : base("Exentials ReCache Command Line")
    {
        Add(new ConnectCommand(_conn));
        Add(new DisconnectCommand(_conn));
        Add(new ShowCommand(_conn));

        Add(new SetCommand(_conn));
        Add(new GetCommand(_conn));
        Add(new DelCommand(_conn));
        Add(new SetHashSetCommand(_conn));
        Add(new GetHashSetCommand(_conn));
        Add(new DelHashSetCommand(_conn));
        Add(new ListCommand(_conn));

        Add(new ClearCommand(_conn));

        Add(new ExitCommand(_conn));
    }
}
