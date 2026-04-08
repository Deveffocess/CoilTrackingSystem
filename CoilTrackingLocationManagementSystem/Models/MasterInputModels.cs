using System.ComponentModel.DataAnnotations;

namespace CoilTrackingLocationManagementSystem.Models;

public class WarehouseInputModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(255)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class BayInputModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string WarehouseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class SubBayInputModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string WarehouseCode { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string BayCode { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class RowInputModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int SubBayId { get; set; }

    [Required]
    [StringLength(100)]
    public string Row { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int Capacity { get; set; }

    [Range(0, int.MaxValue)]
    public int Occupied { get; set; }

    public bool IsActive { get; set; } = true;
}

public class InwardEntryInputModel
{
    [Required]
    [StringLength(50)]
    public string CoilNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string HeatNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Grade { get; set; } = string.Empty;

    [Range(0.01, double.MaxValue)]
    public decimal Thickness { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Width { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Weight { get; set; }

    [Range(1, int.MaxValue)]
    public int LocationId { get; set; }

    [Required]
    [StringLength(50)]
    public string GrnNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(80)]
    public string ReceivedBy { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime ReceivedDate { get; set; } = DateTime.Today;

    [StringLength(255)]
    public string Remarks { get; set; } = string.Empty;
}
