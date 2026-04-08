namespace CoilTrackingLocationManagementSystem.Models;

public class ReportsViewModel
{
    public IReadOnlyList<Coil> Coils { get; set; } = Array.Empty<Coil>();
    public IReadOnlyList<CoilMovement> Movements { get; set; } = Array.Empty<CoilMovement>();
}
