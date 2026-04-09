namespace CoilTrackingLocationManagementSystem.Models
{
    public class TransportarMaster
    {
        public int Id { get; set; }
        public string TransportorName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
