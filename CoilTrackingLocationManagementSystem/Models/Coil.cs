namespace CoilTrackingLocationManagementSystem.Models;

public class Coil
{
    public int Id { get; set; }
    public string CoilNumber { get; set; } = string.Empty;
    public string HeatNumber { get; set; } = string.Empty;
    public string CoilType { get; set; } = string.Empty;
    public decimal Thickness { get; set; }
    public decimal Width { get; set; }
    public decimal Weight { get; set; }
    public string CurrentLocationCode { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;
    public CoilStatus Status { get; set; }
    public DateTime ReceivedDate { get; set; }
}
