using Exentials.ReCache.Models;
using Exentials.ReCache.Server.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exentials.ReCache.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthenticateController : ControllerBase
	{
		private readonly IConfiguration Config;
		private readonly AccountService AccountService;

		public AuthenticateController(
			IConfiguration config,
			AccountService accountService
		)
		{
			Config = config;
			AccountService = accountService;
		}


		/// <summary>  
		/// Hardcoded the User authentication  
		/// </summary>  
		/// <param name="login"></param>  
		/// <returns></returns>  
		private Membership? AuthenticateUser(LoginModel login)
		{
			return (login.Username is not null && login.Password is not null)
				? AccountService.AuthenticateUser(login.Username, login.Password)
				: null;
		}

		/// <summary>  
		/// Login Authenticaton using JWT Token Authentication  
		/// </summary>  
		/// <param name="data"></param>  
		/// <returns></returns>  
		[AllowAnonymous]
		[HttpPost(nameof(Login))]
		[ProducesResponseType(typeof(AuthenticationInfo), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
		public IActionResult Login([FromBody] LoginModel data)
		{
			IActionResult response = Unauthorized();
			Membership? user = AuthenticateUser(data);
			if (user is not null)
			{
				var tokenString = AccountService.GenerateJSONWebToken(user);
				response = Ok(new AuthenticationInfo { Token = tokenString, Message = "Success" });
			}
			return response;
		}

		/// <summary>  
		/// Authorize the Method  
		/// </summary>  
		/// <returns></returns>  
		[HttpGet(nameof(Token))]
		public async Task<ActionResult<IEnumerable<string>>> Token()
		{
			var accessToken = await HttpContext.GetTokenAsync("access_token") ?? string.Empty;
			return new string[] { accessToken };
		}

	}
}
