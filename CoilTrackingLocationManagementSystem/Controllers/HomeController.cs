using System.Diagnostics;
using CoilTrackingLocationManagementSystem.Models;
using CoilTrackingLocationManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoilTrackingLocationManagementSystem.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITrackingRepository _trackingRepository;

    public HomeController(ILogger<HomeController> logger, ITrackingRepository trackingRepository)
    {
        _logger = logger;
        _trackingRepository = trackingRepository;
    }

    public IActionResult Index()
    {
        return View(_trackingRepository.GetDashboard());
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
