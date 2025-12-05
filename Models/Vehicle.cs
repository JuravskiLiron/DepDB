namespace DepDB.Models;

public class Vehicle : BaseDocument
{
    public string Plate { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public string OwnerCitizenId { get; set; } = string.Empty;
    public string OwnerName { get; set; } = string.Empty;

    public bool IsStolen { get; set; }
    public string Notes { get; set; } = string.Empty;
}