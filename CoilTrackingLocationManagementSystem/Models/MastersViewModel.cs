namespace CoilTrackingLocationManagementSystem.Models;

public class MastersViewModel
{
    public IReadOnlyList<Warehouse> Warehouses { get; set; } = Array.Empty<Warehouse>();
    public IReadOnlyList<Bay> Bays { get; set; } = Array.Empty<Bay>();
    public IReadOnlyList<SubBay> SubBays { get; set; } = Array.Empty<SubBay>();
    public IReadOnlyList<StorageLocation> Locations { get; set; } = Array.Empty<StorageLocation>();
    public IReadOnlyList<string> Grades { get; set; } = Array.Empty<string>();
    public IReadOnlyList<string> Suppliers { get; set; } = Array.Empty<string>();
}
