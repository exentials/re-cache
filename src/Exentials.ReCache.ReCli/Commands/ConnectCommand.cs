using Exentials.ReCache.Client;
using System.CommandLine;
using System.CommandLine.Invocation;

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
        hostArgument = new Argument<string>() { Name = "host", Description = "Host name or ip address" };
        hostArgument.SetDefaultValue("localhost");
        AddArgument(hostArgument);

        portOption = new Option<int>("--port", "host port number");
        portOption.SetDefaultValue(443);
        AddOption(portOption);

        routeOption = new Option<string>("--route", "route path");
        routeOption.AddAlias("-r");
        AddOption(routeOption);

        usernameOption = new Option<string>("--username", "Account user name");
        usernameOption.AddAlias("-u");
        usernameOption.SetDefaultValue("default");
        AddOption(usernameOption);

        passwordOption = new Option<string>("--password", "Account password");
        passwordOption.AddAlias("-p");
        AddOption(passwordOption);
    }

    protected override async Task CommandHandler(InvocationContext context)
    {
        var parameters = context.ParseResult;
        string host = parameters.GetValueForArgument(hostArgument);
        int port = parameters.GetValueForOption(portOption);
        string? username = parameters.GetValueForOption(usernameOption);
        string? password = parameters.GetValueForOption(passwordOption);
        string? route = parameters.GetValueForOption(routeOption);
        var cancellationToken = context.GetCancellationToken();

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
