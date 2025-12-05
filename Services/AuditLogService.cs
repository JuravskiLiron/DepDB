using DepDB.Data;
using DepDB.Models;

namespace DepDB.Services;

public class AuditLogService
{
    private readonly IRepository<AuditLog> _repo;

    public AuditLogService(IRepository<AuditLog> repo)
    {
        _repo = repo;
    }

    public async Task Log(string officerId, string officerName, string action, string targetId, string targetType)
    {
        var log = new AuditLog
        {
            OfficerId = officerId,
            OfficerName = officerName,
            Action = action,
            TargetId = targetId,
            TargetType = targetType
        };

        await _repo.InsertAsync(log);
    }
}