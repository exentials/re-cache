using Exentials.ReCache.Client;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal sealed class DisconnectCommand : ReCacheCommandBase
    {
        public DisconnectCommand(ReCacheConnection connection)
            : base(connection, "disconnect", "Disconnect from ReCache host")
        {
        }

        protected override Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            if (Connection.Close())
            {
                Console.WriteLine("Connection closed!");
            }
            return Task.CompletedTask;
        }
    }
}
