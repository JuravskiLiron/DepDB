namespace DepDB.Models;

public class OfficerDashboardViewModel
{
    public Shift? ActiveShift { get; set; }
    public List<IncidentReport> LastReports { get; set; } = new();
    public List<AuditLog> LastChecks { get; set; } = new();
    public UserAccount User { get; set; }

}