using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DepDB.Data;
using DepDB.Models;

namespace DepDB.Controllers;

[Authorize]
public class DutyController : Controller
{
    private readonly IRepository<UserAccount> _users;

    public DutyController(IRepository<UserAccount> users)
    {
        _users = users;
    }

    [HttpPost]
    public async Task<IActionResult> Toggle()
    {
        var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (id == null)
            return Unauthorized("No user ID in claims.");

        var user = await _users.GetByIdAsync(id);
        if (user == null)
            return NotFound();

        user.IsOnDuty = !user.IsOnDuty;
        user.LastDutyChange = DateTime.UtcNow;

        await _users.UpdateAsync(user);

        return RedirectToAction("Dashboard", "Officer");
    }
}