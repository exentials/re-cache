﻿using Exentials.ReCache.Client;
using System.CommandLine.Parsing;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class ExitCommand(ReCacheConnection connection) : ReCacheCommandBase(connection, "exit", "Exit recli")
{
    protected override Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
    {
        if (Connection.Close())
        {
            Console.WriteLine("Connection closed!");
        }
        return Task.CompletedTask;
    }
}
