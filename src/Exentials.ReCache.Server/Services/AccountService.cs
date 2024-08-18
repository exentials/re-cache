using Exentials.ReCache.Models;
using Exentials.ReCache.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Exentials.ReCache.Server.Services;

public sealed class AccountService
{
    private readonly IConfiguration Configuration;
    private readonly List<Membership> Users = new();

    public AccountService(IConfiguration configuration)
    {
        Configuration = configuration;
        Load();
    }

    public void Load()
    {
        var userOptions = new ReCacheUsersOption();
        Configuration.GetSection(ReCacheUsersOption.Users).Bind(userOptions);

        Users.Add(new Membership
        {
            Username = userOptions.AdminUsername,
            Password = userOptions.AdminPassword,
            Roles = ["Admin"]
        });
        Users.Add(new Membership
        {
            Username = userOptions.ClientUsername,
            Password = userOptions.ClientPassword,
            Roles = ["Client"]
        });
    }

    public Membership? AuthenticateUser(string? username, string? password)
    {
        return Users.Where(t => t.Username == username && t.Password == password).SingleOrDefault();
    }

    /// <summary>  
    /// Generate Json Web Token Method  
    /// </summary>  
    /// <param name="membership"></param>  
    /// <returns></returns>  
    public string GenerateJSONWebToken(Membership membership)
    {
        string secretKey = Configuration["Auth:SecretKey"] ?? string.Empty;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, membership.Username)
        };
        foreach (var role in membership.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        var now = DateTime.UtcNow;
        var token = new JwtSecurityToken(
            claims: claims,
            expires: now.AddDays(365),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
