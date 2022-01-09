using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
            if (!ModelState.IsValid)
                return new EmptyResult();
            
            var success = _dataService.SaveProjectTemplate(projectTemplate);
            if (success)
            {
                return ViewComponent("ProjectTemplates");
            }
            
            return Json( new {Message = "A project template already exists with this name." });
        }
        
        [HttpGet]
        public IActionResult UpdateProjectTemplate(int templateId)
        {
            return ViewComponent("EditProjectTemplate", new {id = templateId});
        }
        
        [HttpPost]
        public IActionResult UpdateProjectTemplate(ProjectTemplate projectTemplate)
        {
            if (!ModelState.IsValid)
                return new EmptyResult();

            _dataService.EditProjectTemplate(projectTemplate);
            return ViewComponent("ProjectTemplates");
        }
        
        [HttpPost]
        public IActionResult DeleteProjectTemplate(int id)
        {
            _dataService.DeleteProjectTemplate(id);
            return ViewComponent("ProjectTemplates");
        }
        
        [HttpGet]
        public IActionResult GetProjectFromTemplate(int templateId)
        {
            return ViewComponent("NewProjectFromTemplate", new {id = templateId});
        }
        
        [HttpPost]
        public IActionResult CreateProjectFromTemplate(ProjectTemplate projectTemplate)
        {
            if (!ModelState.IsValid)
                return new EmptyResult();

            var success = _dataService.CreateProjectFromTemplate(projectTemplate);
            if (success)
            {
                return ViewComponent("ProjectTemplates");
            }
            
            return Json( new {Message = "A project already exist with this name." });
        }
        
        [HttpPost]
        public IActionResult CreateProjectFromScratch(ProjectTemplate projectTemplate)
        {
            if (!ModelState.IsValid)
                return new EmptyResult();

            var success = _dataService.CreateProjectFromScratch(projectTemplate);
            if (success)
            {
                return ViewComponent("ProjectTemplates");
            }
            
            return Json( new {Message = "A project already exist with this name." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}