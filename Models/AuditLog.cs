namespace DepDB.Models;

public class AuditLog : BaseDocument
{
    public string OfficerId { get; set; } = string.Empty;
    public string OfficerName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;
    public string TargetId { get; set; } = string.Empty;
    public string TargetType { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

}