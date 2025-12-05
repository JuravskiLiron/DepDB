using DepDB.Data;
using DepDB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LspdCad.Controllers;

[Authorize(Roles = Roles.Officer + "," + Roles.Supervisor + "," + Roles.Admin)]
public class CitizensController : Controller
{
    private readonly IRepository<Citizen> _citizens;

    public CitizensController(IRepository<Citizen> citizens)
    {
        _citizens = citizens;
    }

    // GET: /Citizens
    public async Task<IActionResult> Index(string? search, int page = 1, int pageSize = 30)
    {
        int skip = (page - 1) * pageSize;

        var list = string.IsNullOrWhiteSpace(search)
            ? await _citizens.GetAllAsync(skip, pageSize)
            : await _citizens.FindAsync(
                c => c.FullName.ToLower().Contains(search.ToLower())
                     || c.NationalId.Contains(search),
                skip, pageSize);

        ViewBag.Search = search;
        ViewBag.Page = page;
        return View(list);
    }

    // GET: /Citizens/Details/5
    public async Task<IActionResult> Details(string id)
    {
        var citizen = await _citizens.GetByIdAsync(id);
        if (citizen == null) return NotFound();
        return View(citizen);
    }

    // GET: /Citizens/Create
    [Authorize(Roles = $"{Roles.Admin},{Roles.Officer}")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Officer}")]
    public async Task<IActionResult> Create(Citizen citizen)
    {
        if (!ModelState.IsValid)
            return View(citizen);

        await _citizens.InsertAsync(citizen);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Citizens/Edit/5
    [Authorize(Roles = $"{Roles.Admin},{Roles.Officer}")]
    public async Task<IActionResult> Edit(string id)
    {
        var citizen = await _citizens.GetByIdAsync(id);
        if (citizen == null) return NotFound();
        return View(citizen);
    }

    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.Officer}")]
    public async Task<IActionResult> Edit(string id, Citizen citizen)
    {
        if (id != citizen.Id) return BadRequest();

        if (!ModelState.IsValid)
            return View(citizen);

        await _citizens.UpdateAsync(citizen);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Citizens/Delete/5
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> Delete(string id)
    {
        var citizen = await _citizens.GetByIdAsync(id);
        if (citizen == null) return NotFound();
        return View(citizen);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _citizens.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
