namespace CoilTrackingLocationManagementSystem.Models;

public class DataAccessOptions
{
    public const string SectionName = "DataAccess";

    public string Provider { get; set; } = "InMemory";
}
