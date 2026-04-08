namespace CoilTrackingLocationManagementSystem.Models;

public class TransactionsViewModel
{
    public IReadOnlyList<Coil> InwardCoils { get; set; } = Array.Empty<Coil>();
    public IReadOnlyList<CoilMovement> Movements { get; set; } = Array.Empty<CoilMovement>();
    public IReadOnlyList<StorageLocation> Locations { get; set; } = Array.Empty<StorageLocation>();
    public IReadOnlyList<string> Grades { get; set; } = Array.Empty<string>();
}
