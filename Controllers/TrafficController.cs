using DepDB.Data;
using DepDB.Models;
using DepDB.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DepDB.Controllers;

[Authorize(Roles = $"{Roles.Officer},{Roles.Supervisor},{Roles.Admin}")]
public class TrafficController : Controller
{
    private readonly IRepository<Vehicle> _vehicles;
    private readonly IRepository<Citizen> _citizens;
    private readonly IRepository<IncidentReport> _reports;
    private readonly AuditLogService _audit;

    public TrafficController(
        IRepository<Vehicle> vehicles,
        IRepository<Citizen> citizens,
        IRepository<IncidentReport> reports,
        AuditLogService audit)
    {
        _vehicles = vehicles;
        _citizens = citizens;
        _reports = reports;
        _audit = audit;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Проверка машины
    public async Task<IActionResult> VehicleCheck(string plate)
    {
        if (string.IsNullOrEmpty(plate))
            return View(null);

        var vehicle = (await _vehicles.FindAsync(v => v.Plate.ToLower() == plate.ToLower()))
            .FirstOrDefault();

        await _audit.Log(
            UserId(), User.Identity!.Name!, "Vehicle Check", plate, "Vehicle"
        );

        return View(vehicle);
    }

    // Проверка гражданина
    public async Task<IActionResult> CitizenCheck(string name)
    {
        if (string.IsNullOrEmpty(name))
            return View(null);

        var citizen = (await _citizens.FindAsync(c =>
            c.FullName.ToLower().Contains(name.ToLower())))
            .FirstOrDefault();

        await _audit.Log(
            UserId(), User.Identity!.Name!, "Citizen Check", name, "Citizen"
        );

        return View(citizen);
    }

    // Создать рапорт после Traffic Stop
    [HttpPost]
    public async Task<IActionResult> CreateReport(string type, string description, string citizenId, string vehicleId)
    {
        var officerId = UserId();
        var officerName = User.Identity!.Name!;

        var report = new IncidentReport
        {
            OfficerId = officerId,
            OfficerName = officerName,
            Type = IncidentType.TrafficStop,
            Title = "Traffic Stop Report",
            Description = description,
            CitizenId = citizenId,
            VehicleId = vehicleId,
            Status = IncidentStatus.Open,
            Location = "Unknown"
        };

        await _reports.InsertAsync(report);

        await _audit.Log(officerId, officerName, "Created Traffic Stop Report", report.Id, "Incident");

        return RedirectToAction("Dashboard", "Officer");
    }

    private string UserId() => User.FindFirst("nameidentifier")!.Value;
}
