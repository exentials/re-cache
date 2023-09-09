using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal class ShowTokenCommand : ReCacheCommandBase
    {
        public ShowTokenCommand(ReCacheConnection connection)
            : base(connection, "token")
        {            
        }

        protected override Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            Console.WriteLine(client.AuthToken);
            return Task.CompletedTask;
        }

    }
}
