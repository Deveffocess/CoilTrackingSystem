namespace CoilTrackingLocationManagementSystem.Models;

public class StorageLocation
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Warehouse { get; set; } = string.Empty;
    public string Bay { get; set; } = string.Empty;
    public string SubBay { get; set; } = string.Empty;
    public string Row { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public int Occupied { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    public string HierarchyPath => $"{Warehouse} / {Bay} / {SubBay} / {Row}";
}
