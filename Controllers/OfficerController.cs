using System.Security.Claims;
using DepDB.Data;
using DepDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

namespace DepDB.Controllers;

[Authorize]
public class OfficerController : Controller
{
    private readonly IRepository<UserAccount> _users;
    private readonly IRepository<Shift> _shifts;
    private readonly IRepository<IncidentReport> _reports;
    private readonly IRepository<AuditLog> _logs;

    public OfficerController(
        IRepository<UserAccount> users,
        IRepository<Shift> shifts,
        IRepository<IncidentReport> reports,
        IRepository<AuditLog> logs)
    {
        _users = users;
        _shifts = shifts;
        _reports = reports;
        _logs = logs;
    }

    public async Task<IActionResult> Dashboard()
    {
        // 1. Берём userId из клейма безопасно
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            // нет клейма -> выкидываем на логин
            await HttpContext.SignOutAsync("LspdCookie");
            return RedirectToAction("Login", "Account");
        }

        // 2. Грузим пользователя из Mongo
        var user = await _users.GetByIdAsync(userId);
        if (user == null)
        {
            // в базе нет такого пользователя -> на логин
            await HttpContext.SignOutAsync("LspdCookie");
            return RedirectToAction("Login", "Account");
        }

        // 3. Всё как у тебя было, только с тем же userId
        var activeShift = (await _shifts.FindAsync(s => s.OfficerId == userId && s.EndTime == null))
            .FirstOrDefault();

        var lastReports = (await _reports.FindAsync(r => r.OfficerId == userId))
            .OrderByDescending(r => r.CreatedAtUtc)
            .Take(5)
            .ToList();

        var lastChecks = (await _logs.FindAsync(l => l.OfficerId == userId))
            .OrderByDescending(l => l.CreatedAtUtc)
            .Take(5)
            .ToList();

        var vm = new OfficerDashboardViewModel
        {
            User = user,
            ActiveShift = activeShift,
            LastReports = lastReports,
            LastChecks = lastChecks
        };

        return View(vm);
    }
}
