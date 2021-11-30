using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;
using ProductionPlanner.Services;
using ProductionPlanner.ViewModels;

namespace ProductionPlanner.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPlannerService _plannerService;

        public HomeController(ILogger<HomeController> logger, IPlannerService plannerService)
        {
            _logger = logger;
            _plannerService = plannerService;
        }

        public IActionResult Index()
        {
            PlannerViewModel plannerViewModel = new();
            var mockData = _plannerService.MockData();
            plannerViewModel.Weeks = _plannerService.AssignProjects(mockData);
            
            return View(plannerViewModel);
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

}