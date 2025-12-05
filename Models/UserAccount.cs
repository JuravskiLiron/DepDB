using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DepDB.Models;

public class UserAccount : BaseDocument
{
   
    public string FullName { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = Roles.Officer;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public bool IsOnDuty { get; set; } = false;
    public DateTime? LastDutyChange { get; set; }

}