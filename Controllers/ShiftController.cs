using DepDB.Data;
using DepDB.Models;
using DepDB.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepDB.Controllers;

[Authorize(Roles = $"{Roles.Officer},{Roles.Supervisor},{Roles.Admin}")]
public class ShiftController : Controller
{
    private readonly IRepository<Shift> _shifts;
    private readonly AuditLogService _audit;

    public ShiftController(IRepository<Shift> shifts, AuditLogService audit)
    {
        _shifts = shifts;
        _audit = audit;
    }

    // Начать смену
    public async Task<IActionResult> Start()
    {
        var officerId = User.FindFirst("nameidentifier")!.Value;
        var officerName = User.Identity!.Name!;

        // Проверяем — нет ли уже активной смены
        var active = (await _shifts.FindAsync(s => s.OfficerId == officerId && s.EndTime == null))
            .FirstOrDefault();

        if (active != null)
            return RedirectToAction("Dashboard", "Officer");

        var shift = new Shift
        {
            OfficerId = officerId,
            OfficerName = officerName,
            StartTime = DateTime.UtcNow
        };

        await _shifts.InsertAsync(shift);

        await _audit.Log(officerId, officerName, "Started Shift", shift.Id, "Shift");

        return RedirectToAction("Dashboard", "Officer");
    }

    // Завершить смену
    public async Task<IActionResult> End()
    {
        var officerId = User.FindFirst("nameidentifier")!.Value;
        var officerName = User.Identity!.Name!;

        var active = (await _shifts.FindAsync(s => s.OfficerId == officerId && s.EndTime == null))
            .FirstOrDefault();

        if (active == null)
            return RedirectToAction("Dashboard", "Officer");

        active.EndTime = DateTime.UtcNow;

        await _shifts.UpdateAsync(active);

        await _audit.Log(officerId, officerName, "Ended Shift", active.Id, "Shift");

        return RedirectToAction("Dashboard", "Officer");
    }

    // История смен
    public async Task<IActionResult> History()
    {
        var id = User.FindFirst("nameidentifier")!.Value;
        var list = await _shifts.FindAsync(s => s.OfficerId == id);
        list = list.OrderByDescending(s => s.StartTime).ToList();
        return View(list);
    }
}
