using Exentials.ReCache.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication3.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly AccountService _accountService;

        [BindProperty]
        public string Username { get; set; } = string.Empty;
        [BindProperty]
        public string Password { get; set; } = string.Empty;
        public string? Token { get; set; }

        public IndexModel(ILogger<IndexModel> logger, AccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(Username) && !string.IsNullOrEmpty(Password))
            {
                var member = _accountService.AuthenticateUser(Username, Password);
                if (member is not null)
                {
                    Token = _accountService.GenerateJSONWebToken(member);
                    _logger.LogInformation("Authentication token request.");
                }
            }
            return Page();
        }
    }
}