namespace CoilTrackingLocationManagementSystem.Models;

public class DashboardViewModel
{
    public int TotalCoils { get; set; }
    public int StoredCoils { get; set; }
    public int InspectionPending { get; set; }
    public int DispatchReady { get; set; }
    public IReadOnlyList<Coil> RecentCoils { get; set; } = Array.Empty<Coil>();
    public IReadOnlyList<StorageLocation> Locations { get; set; } = Array.Empty<StorageLocation>();
    public IReadOnlyList<CoilMovement> RecentMovements { get; set; } = Array.Empty<CoilMovement>();
}
