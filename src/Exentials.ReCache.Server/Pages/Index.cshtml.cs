using Exentials.ReCache.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Exentials.ReCache.Server.Pages
{
    public class IndexModel(ILogger<IndexModel> logger, AccountService accountService) : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        public string? Token { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                var member = accountService.AuthenticateUser(Username, Password);
                if (member is not null)
                {
                    Token = accountService.GenerateJSONWebToken(member);
                    logger.LogInformation("Authentication token request.");
                }
            }
            return Page();
        }
    }
}