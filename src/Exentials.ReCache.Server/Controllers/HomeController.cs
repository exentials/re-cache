using Exentials.ReCache.Models;
using Exentials.ReCache.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Exentials.ReCache.Server.Controllers;

[Authorize]
public class HomeController(AccountService account) : Controller
{
    private readonly AccountService _account = account;

    [AllowAnonymous]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult Index([FromForm] LoginModel model)
    {
        var member = _account.AuthenticateUser(model.Username, model.Password ?? "");
        if (member is not null)
        {
            ViewBag.Token = _account.GenerateJSONWebToken(member);
        }
        return View();
    }
}
