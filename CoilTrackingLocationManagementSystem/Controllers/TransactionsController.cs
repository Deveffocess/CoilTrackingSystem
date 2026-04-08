using CoilTrackingLocationManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using CoilTrackingLocationManagementSystem.Models;

namespace CoilTrackingLocationManagementSystem.Controllers;

public class TransactionsController : Controller
{
    private readonly ITrackingRepository _trackingRepository;
    private readonly ILogger<TransactionsController> _logger;

    public TransactionsController(ITrackingRepository trackingRepository, ILogger<TransactionsController> logger)
    {
        _trackingRepository = trackingRepository;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View(_trackingRepository.GetTransactions());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateInwardEntry(InwardEntryInputModel input)
    {
        try
        {
            _trackingRepository.CreateInwardEntry(input);
            TempData["StatusMessage"] = "Inward entry saved successfully.";
            TempData["StatusType"] = "success";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Inward entry save failed for coil {CoilNumber}", input.CoilNumber);
            TempData["StatusMessage"] = ex.InnerException is null ? ex.Message : $"{ex.Message} | Detail: {ex.InnerException.Message}";
            TempData["StatusType"] = "danger";
        }

        return RedirectToAction(nameof(Index));
    }
}
