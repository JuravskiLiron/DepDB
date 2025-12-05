namespace DepDB.Models;

public enum IncidentType
{
    TrafficStop,
    Citation,
    Arrest,
    VehicleCheck,
    PersonCheck,
    CallResponse,
    Other
}

public enum IncidentStatus
{
    Open,
    Closed
}

public class IncidentReport : BaseDocument
{
    public IncidentType Type { get; set; } = IncidentType.Other;

    public string OfficerId { get; set; } = string.Empty;
    public string OfficerName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string CitizenId { get; set; } = string.Empty;
    public string VehicleId { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public IncidentStatus Status { get; set; } = IncidentStatus.Open;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

}