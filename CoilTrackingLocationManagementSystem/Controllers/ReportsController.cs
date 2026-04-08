using CoilTrackingLocationManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoilTrackingLocationManagementSystem.Controllers;

public class ReportsController : Controller
{
    private readonly ITrackingRepository _trackingRepository;

    public ReportsController(ITrackingRepository trackingRepository)
    {
        _trackingRepository = trackingRepository;
    }

    public IActionResult Index()
    {
        return View(_trackingRepository.GetReports());
    }
}
