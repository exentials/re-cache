using Exentials.ReCache.ReCli.Commands;
using System.CommandLine;

class Program
{
    static async Task<int> Main(string[] _)
    {
        var recli = new ReCacheRootCommand();

        Console.WriteLine("Welcome to ReCache Cli");
        Console.WriteLine("Digit -h for command help.");
        Console.Write("> ");
        string? cmd;
        while ((cmd = Console.ReadLine()) != "exit")
        {
            if (!string.IsNullOrWhiteSpace(cmd))
            {
                await recli.InvokeAsync(cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
            Console.Write("> ");
        }
        Console.WriteLine("Bye!");
        return 0;
    }
}