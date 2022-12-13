using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal class ClearCommand : ReCacheCommandBase
    {
        private readonly NameSpaceOption namespaceOption = new();

        public ClearCommand(ReCacheConnection connection)
            : base(connection, "clear", "Clear cache")
        {
            AddOption(namespaceOption);
        }

        protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            var nameSpace = parameters.GetValueForOption(namespaceOption);
            await client.Clear(nameSpace);
        }
    }
}
