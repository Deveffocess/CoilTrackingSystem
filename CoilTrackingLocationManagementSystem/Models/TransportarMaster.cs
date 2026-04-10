namespace CoilTrackingLocationManagementSystem.Models
{
    public class TransportarMaster
    {
        public int Id { get; set; }
        public string TransportorName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedUser { get; set; } // Added New Code By Danish 10042026 
        public DateTime ModifiedDate { get; set; }       
        public string ModifiedUser{ get; set; } // Added New Code By Danish 10042026
    }
}
