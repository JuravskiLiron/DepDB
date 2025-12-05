namespace DepDB.Models;

public class Shift : BaseDocument
{
    public string OfficerId { get; set; } = string.Empty;
    public string OfficerName { get; set; } = string.Empty;

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public bool IsActive => EndTime == null;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

}