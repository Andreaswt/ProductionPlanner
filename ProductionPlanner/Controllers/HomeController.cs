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
            return View();
        }
        
        public ActionResult GetChartData()
        {
            List<Project> taskList = new();
            var mockProjects = _plannerService.MockData();

            var assignedProjects = _plannerService.AssignProjects(mockProjects);

            string json = _plannerService.ChartJsonGenerator(assignedProjects);

            return Content(json);
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