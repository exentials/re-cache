using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal sealed class GetCommand : ReCacheCommandBase
    {
        private readonly KeyArgument keyArg = new();
        private readonly NameSpaceOption namespaceOption = new();

        public GetCommand(ReCacheConnection connection)
            : base(connection, "get")
        {
            AddArgument(keyArg);
            AddOption(namespaceOption);
        }

        protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            var key = parameters.GetValueForArgument(keyArg);
            var nameSpace = parameters.GetValueForOption(namespaceOption);

            var value = await client.GetAsync(key, nameSpace);
            Console.WriteLine($"{value}");
        }
    }
}
