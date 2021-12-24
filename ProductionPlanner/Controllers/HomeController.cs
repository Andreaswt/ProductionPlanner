﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductionPlanner.Data;
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
        private readonly IDataService _dataService;

        public HomeController(ILogger<HomeController> logger, IPlannerService plannerService, IDataService dataService)
        {
            _logger = logger;
            _plannerService = plannerService;
            _dataService = dataService;
        }

        public IActionResult Index()
        {
            PlannerViewModel plannerViewModel = new();
            
            // var mock = _plannerService.MockData();
            //
            // // Save to database
            // _dataService.SaveWeeks(_plannerService.AssignProjects(mock));

            // Get from database
            plannerViewModel.Weeks = _dataService.GetWeeks();

            return View(plannerViewModel);
        }

        [HttpPost]
        public IActionResult NewProjectTemplate(ProjectTemplate projectTemplate)
        {
            // PlannerViewModel plannerViewModel = new();
            // plannerViewModel.Weeks = _dataService.GetWeeks();
            Console.Write("Hej");
            return new EmptyResult();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}