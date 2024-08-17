namespace Exentials.ReCache.Server.Models;

public class ReCacheUsersOption
{
    public const string Users = "Users";
    public string AdminUsername { get; set; } = string.Empty;
    public string AdminPassword { get; set; } = string.Empty;
    public string ClientUsername { get; set; } = string.Empty;
    public string ClientPassword { get; set; } = string.Empty;
}
