using System.Security.Claims;
using DepDB.Data;
using DepDB.Models;
using DepDB.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace DepDB.Controllers;

public class AccountController : Controller
{
    private readonly IRepository<UserAccount> _users;
    private readonly PasswordHasher _hasher;

    public AccountController(IRepository<UserAccount> users, PasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = (await _users.FindAsync(u => u.Username == username && u.IsActive))
            .FirstOrDefault();

        if (user == null || !_hasher.Verify(password, user.PasswordHash))
        {
            ViewBag.Error = "Invalid username or password";
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var identity = new ClaimsIdentity(claims, "LspdCookie");
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync("LspdCookie", principal);

        return RedirectToAction("Dashboard", "Officer"); // <-- ВОТ ТУТ РЕДИРЕКТ
    }


    

    [HttpPost]
    public async Task<IActionResult> Register(string username, string password, string role)
    {
        var existing = (await _users.FindAsync(u => u.Username == username)).FirstOrDefault();
        if (existing != null)
        {
            ViewBag.Error = "User already exists";
            return View();
        }

        var user = new UserAccount
        {
            Username = username,
            PasswordHash = _hasher.Hash(password),
            Role = role,
            IsActive = true
        };

        await _users.InsertAsync(user);
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("LspdCookie");
        return RedirectToAction("Login");
    }

    public IActionResult AccessDenied()
    {
        return View();
    }
}
