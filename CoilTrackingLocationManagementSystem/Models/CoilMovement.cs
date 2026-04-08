namespace CoilTrackingLocationManagementSystem.Models;

public class CoilMovement
{
    public int Id { get; set; }
    public string CoilNumber { get; set; } = string.Empty;
    public string FromLocation { get; set; } = string.Empty;
    public string ToLocation { get; set; } = string.Empty;
    public string Activity { get; set; } = string.Empty;
    public string ReferenceNumber { get; set; } = string.Empty;
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTime MovementDate { get; set; }
}
