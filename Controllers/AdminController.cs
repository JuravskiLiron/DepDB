using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DepDB.Data;
using DepDB.Models;
using DepDB.Services;

namespace DepDB.Controllers;

[Authorize(Roles = Roles.Admin)]
public class AdminController : Controller
{
    private readonly IRepository<UserAccount> _users;
    private readonly PasswordHasher _hasher;

    public AdminController(IRepository<UserAccount> users, PasswordHasher hasher)
    {
        _users = users;
        _hasher = hasher;
    }

    // Список пользователей
    public async Task<IActionResult> Index()
    {
        var all = await _users.GetAllAsync();
        return View(all);
    }

    // Создать пользователя
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(string fullName, string username, string password, string role)
    {
        var existing = (await _users.FindAsync(u => u.Username == username)).FirstOrDefault();
        if (existing != null)
        {
            ViewBag.Error = "User already exists";
            return View();
        }

        var user = new UserAccount
        {
            FullName = fullName,
            Username = username,
            PasswordHash = _hasher.Hash(password),
            Role = role,
            IsActive = true
        };

        await _users.InsertAsync(user);
        return RedirectToAction("Index");
    }


    // Редактировать пользователя
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UserAccount model)
    {
        var user = await _users.GetByIdAsync(model.Id);
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.Username = model.Username;
        user.Role = model.Role;
        user.IsActive = model.IsActive;

        await _users.UpdateAsync(user);

        return RedirectToAction("Index");
    }

    // Удалить
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        await _users.DeleteAsync(id);
        return RedirectToAction("Index");
    }
}
