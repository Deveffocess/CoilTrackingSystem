using CoilTrackingLocationManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using CoilTrackingLocationManagementSystem.Models;

namespace CoilTrackingLocationManagementSystem.Controllers;

public class MastersController : Controller
{
    private readonly ITrackingRepository _trackingRepository;
    private readonly ILogger<MastersController> _logger;

    public MastersController(ITrackingRepository trackingRepository, ILogger<MastersController> logger)
    {
        _trackingRepository = trackingRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(_trackingRepository.GetMasters());
    }

    public IActionResult Warehouses()
    {
        return View(new MastersViewModel
        {
            Warehouses = _trackingRepository.GetWarehouses()
        });
    }

    public IActionResult Bays()
    {
        return View(new MastersViewModel
        {
            Warehouses = _trackingRepository.GetWarehouses(),
            Bays = _trackingRepository.GetBays()
        });
    }

    public IActionResult SubBays()
    {
        return View(new MastersViewModel
        {
            Warehouses = _trackingRepository.GetWarehouses(),
            Bays = _trackingRepository.GetBays(),
            SubBays = _trackingRepository.GetSubBays()
        });
    }

    public IActionResult Rows()
    {
        return View(new MastersViewModel
        {
            Warehouses = _trackingRepository.GetWarehouses(),
            Bays = _trackingRepository.GetBays(),
            SubBays = _trackingRepository.GetSubBays(),
            Locations = _trackingRepository.GetRows()
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateWarehouse(WarehouseInputModel input)
    {
        return Execute(nameof(Warehouses), () => _trackingRepository.CreateWarehouse(input), "Warehouse created successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateWarehouse(WarehouseInputModel input)
    {
        return Execute(nameof(Warehouses), () => _trackingRepository.UpdateWarehouse(input), "Warehouse updated successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteWarehouse(int id)
    {
        return Execute(nameof(Warehouses), () => _trackingRepository.DeleteWarehouse(id), "Warehouse deleted successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateBay(BayInputModel input)
    {
        return Execute(nameof(Bays), () => _trackingRepository.CreateBay(input), "Bay created successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateBay(BayInputModel input)
    {
        return Execute(nameof(Bays), () => _trackingRepository.UpdateBay(input), "Bay updated successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteBay(int id)
    {
        return Execute(nameof(Bays), () => _trackingRepository.DeleteBay(id), "Bay deleted successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateSubBay(SubBayInputModel input)
    {
        return Execute(nameof(SubBays), () => _trackingRepository.CreateSubBay(input), "Sub-bay created successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateSubBay(SubBayInputModel input)
    {
        return Execute(nameof(SubBays), () => _trackingRepository.UpdateSubBay(input), "Sub-bay updated successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteSubBay(int id)
    {
        return Execute(nameof(SubBays), () => _trackingRepository.DeleteSubBay(id), "Sub-bay deleted successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateRow(RowInputModel input)
    {
        return Execute(nameof(Rows), () => _trackingRepository.CreateRow(input), "Row created successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateRow(RowInputModel input)
    {
        return Execute(nameof(Rows), () => _trackingRepository.UpdateRow(input), "Row updated successfully.");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteRow(int id)
    {
        return Execute(nameof(Rows), () => _trackingRepository.DeleteRow(id), "Row deleted successfully.");
    }

    private IActionResult Execute(string actionName, Action action, string successMessage)
    {
        try
        {
            action();
            TempData["StatusMessage"] = successMessage;
            TempData["StatusType"] = "success";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Master action failed on {ActionName}", actionName);
            TempData["StatusMessage"] = ex.InnerException is null ? ex.Message : $"{ex.Message} | Detail: {ex.InnerException.Message}";
            TempData["StatusType"] = "danger";
        }

        return RedirectToAction(actionName);
    }
}
