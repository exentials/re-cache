using Exentials.ReCache.ReCli.Commands;
using System.CommandLine;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var recli = new ReCacheRootCommand();

        string? cmd;
        Console.WriteLine("Welcome to ReCache Cli");
        Console.WriteLine("Digit -h for command help.");
        Console.Write("> ");
        while ((cmd = Console.ReadLine()) != "exit")
        {
            if (!string.IsNullOrWhiteSpace(cmd))
            {
                await recli.InvokeAsync(cmd.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            }
            Console.Write("> ");
        }
        await recli.InvokeAsync(cmd);
        Console.WriteLine("Bye!");
        return 0;
    }
}