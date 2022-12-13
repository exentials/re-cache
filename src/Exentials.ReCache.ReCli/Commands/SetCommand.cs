using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal sealed class SetCommand : ReCacheCommandBase
    {
        private readonly KeyArgument keyArg = new();
        private readonly ValueArgument valueArg = new();
        private readonly NameSpaceOption namespaceOption = new();
        private readonly AbsoluteOption absoluteOption = new();
        private readonly SlidingOption slidingOption = new();

        public SetCommand(ReCacheConnection connection)
            : base(connection, "set")
        {
            AddArgument(keyArg);
            AddArgument(valueArg);
            AddOption(namespaceOption);
            AddOption(absoluteOption);
            AddOption(slidingOption);
        }

        protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            var key = parameters.GetValueForArgument(keyArg);
            var value = parameters.GetValueForArgument(valueArg);
            var absolute = parameters.GetValueForOption(absoluteOption);
            var sliding = parameters.GetValueForOption(slidingOption);

            var nameSpace = parameters.GetValueForOption(namespaceOption);

            if (await client.SetAsync(key, value, absolute, sliding, nameSpace))
            {
                Console.WriteLine($"{value} cached");
            }
        }

    }
}
