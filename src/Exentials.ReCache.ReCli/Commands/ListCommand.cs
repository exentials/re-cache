using Exentials.ReCache.Client;
using Exentials.ReCache.ReCli.Parameters;
using Google.Protobuf.WellKnownTypes;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands
{
    internal sealed class ListCommand : ReCacheCommandBase
    {
        private readonly Argument<ListArgument> argument = new(() => ListArgument.Keys);
        private readonly Option<KeyType> typeOption = new("--type", "Cache structure type");
        private readonly NameSpaceOption namespaceOption = new();
        public ListCommand(ReCacheConnection connection)
            : base(connection, "list", "List the keys of a cache structure")
        {
            AddAlias("ls");
            AddArgument(argument);
            typeOption.SetDefaultValue(KeyType.Set);
            typeOption.AddAlias("-t");
            AddOption(typeOption);
            AddOption(namespaceOption);
        }

        protected override async Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
        {
            var listArgument = parameters.GetValueForArgument(argument);
            var type = parameters.GetValueForOption(typeOption);
            var nameSpace = parameters.GetValueForOption(namespaceOption);

            if (listArgument == ListArgument.Keys)
            {
                if (type == KeyType.Set)
                {
                    Console.WriteLine($"Keys for namespace: {nameSpace ?? "<empty>"}");
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
            else if (listArgument == ListArgument.Namespaces)
            {
                if (type == KeyType.Set)
                {
                    Console.WriteLine($"Namespaces in dictionary:");
                    foreach (var ns in await client.ListDictionaryNamespacesAsync())
                    {
                        Console.WriteLine($"{ns}");
                    }
                }
                else if (type == KeyType.HashSet)
                {                    
                    Console.WriteLine($"Namespaces in hashset:");
                    foreach (var ns in await client.ListHashSetNamespacesAsync())
                    {
                        Console.WriteLine($"{ns}");
                    }
                }
                else
                {
                    Console.WriteLine($"Unknow type.");
                }

            }
            else
            {
                Console.WriteLine($"Unknow argument.");
            }

        }
    }
}
