namespace CoilTrackingLocationManagementSystem.Models;

public class Bay
{
    public int Id { get; set; }
    public string WarehouseCode { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
}
