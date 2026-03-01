using Exentials.ReCache.ReCli.Commands;
using System.Reflection;

class Program
{
	static async Task<int> Main(string[] _)
	{
		var version = Assembly.GetExecutingAssembly().GetName().Version;

		Console.WriteLine($"Welcome to ReCache Cli {version}");
		Console.WriteLine("Digit -h for command help.");
		Console.Write("> ");

		string? cmd;
		var recli = new ReCacheRootCommand();
		while ((cmd = Console.ReadLine()) != "exit")
		{
			if (!string.IsNullOrWhiteSpace(cmd))
			{				
				var parsedResult = recli.Parse(cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries));
				await parsedResult.InvokeAsync();
			}
			Console.Write("> ");
		}
		Console.WriteLine("Bye!");
		return 0;
	}
}