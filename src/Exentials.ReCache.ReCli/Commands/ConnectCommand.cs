using Exentials.ReCache.Client;
using System.CommandLine;

namespace Exentials.ReCache.ReCli.Commands;

internal sealed class ConnectCommand : ReCacheCommandBase
{
	private readonly Argument<string> hostArgument;
	private readonly Option<int> portOption;
	private readonly Option<string> routeOption;
	private readonly Option<string> usernameOption;
	private readonly Option<string> passwordOption;

	public ConnectCommand(ReCacheConnection connection)
		: base(connection, "connect")
	{
		hostArgument = new Argument<string>("host")
		{
			Description = "Host name or ip address",
			DefaultValueFactory = _ => "localhost"
		};
		Arguments.Add(hostArgument);

		portOption = new Option<int>("--port")
		{
			Description = "Host port number",
			DefaultValueFactory = _ => 443
		};
		Options.Add(portOption);

		routeOption = new Option<string>("--route");
		routeOption.Description = "Route to connect to";
		routeOption.Aliases.Add("-r");
		Options.Add(routeOption);

		usernameOption = new Option<string>("--username");
		usernameOption.Description = "Account username";
		usernameOption.Aliases.Add("-u");
		usernameOption.DefaultValueFactory = _ => "default";
		Options.Add(usernameOption);

		passwordOption = new Option<string>("--password");
		passwordOption.Description = "Account password";
		passwordOption.Aliases.Add("-p");
		Options.Add(passwordOption);
	}

	protected override async Task CommandHandler(ParseResult parameters, CancellationToken cancellationToken)
	{
		string host = parameters.GetValue(hostArgument);
		int port = parameters.GetValue(portOption);
		string? username = parameters.GetValue(usernameOption);
		string? password = parameters.GetValue(passwordOption);
		string? route = parameters.GetValue(routeOption);

		if (await Connection.Connect(host, port, username, password, route, cancellationToken))
		{
			Console.WriteLine("Connected!");
		}
		else
		{
			Console.WriteLine("Connection refused!");
		}
	}

}
