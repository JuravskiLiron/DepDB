namespace DepDB.Models;

public class Citizen : BaseDocument
{
    public string FullName { get; set; } = string.Empty;
    public string DateOfBirth { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public string NationalId { get; set; } = string.Empty;  // ← ДОБАВИЛ
    public bool IsWanted { get; set; }                      // ← ДОБАВИЛ

    public string Notes { get; set; } = string.Empty;
}