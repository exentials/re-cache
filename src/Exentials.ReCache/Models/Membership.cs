namespace Exentials.ReCache.Models;

public class Membership
{
    /// <summary>
    /// User name 
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// Password
    /// </summary>
    public string Password { get; set; } = string.Empty;
    /// <summary>
    /// Authorization roles
    /// </summary>
    public string[] Roles { get; set; } = [];

    /// <summary>
    /// Get if member is in role
    /// </summary>
    /// <param name="role">authorization role</param>
    /// <returns>Return true or false if member is in role</returns>
    public bool IsInRole(string role)
    {
        return Roles?.Contains(role) ?? false;
    }
}
