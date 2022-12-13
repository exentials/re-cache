using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal sealed class ListCommand : ReCacheCommandBase
    {
        private readonly Option<KeyType> typeOption = new("--type", "Cache structure type");
        private readonly NameSpaceOption namespaceOption = new();
        public ListCommand(ReCacheConnection connection)
            : base(connection, "list", "List the keys of a cache structure")
        {
            AddAlias("ls");
            typeOption.SetDefaultValue(KeyType.Set);
            typeOption.AddAlias("-t");
            AddOption(typeOption);
            AddOption(namespaceOption);
        }

        protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            var type = parameters.GetValueForOption(typeOption);
            var nameSpace = parameters.GetValueForOption(namespaceOption);

            if (type == KeyType.Set)
            {
                Console.WriteLine($"Keys for namespace: {nameSpace}");
                foreach (var key in await client.ListDictionaryAsync(nameSpace))
                {
                    Console.WriteLine($"{key}");
                }
            }
            else if (type == KeyType.HashSet)
            {
                Console.WriteLine($"Keys for namespace: {nameSpace}");
                foreach (var key in await client.ListHashSetAsync(nameSpace))
                {
                    Console.WriteLine($"{key}");
                }
            }
            else
            {
                Console.WriteLine($"Unknow type.");
            }

        }
    }
}
