namespace Exentials.ReCache.Server.Models;

public class AuthOptions
{
    public const string Auth = "Auth";
    public string SecretKey { get; set; } = string.Empty;
}
