using Exentials.ReCache.Client;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class ShowCommand : ReCacheCommandBase
{
    public ShowCommand(ReCacheConnection connection)
        : base(connection, "show")
    {
        AddCommand(new ShowTokenCommand(connection));
    }

}
