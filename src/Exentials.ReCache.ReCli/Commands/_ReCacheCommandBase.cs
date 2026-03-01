using Exentials.ReCache.Client;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal abstract class ReCacheCommandBase : Command
{
	protected readonly ReCacheConnection Connection;
	public ReCacheCommandBase(ReCacheConnection connection, string name, string? description = null) : base(name, description)
	{
		Connection = connection;
		this.SetAction(CommandHandler);
	}

	protected virtual async Task CommandHandler(ParseResult parameters, CancellationToken cancellationToken)
	{
		if (Connection.IsConnected && Connection.Client is not null)
		{
			await Invoke(Connection.Client, parameters, cancellationToken);
		}
		else
		{
			Console.WriteLine("You must connect first.");
		}
	}

	protected virtual Task Invoke(ReCacheClient client, ParseResult parameters, CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
