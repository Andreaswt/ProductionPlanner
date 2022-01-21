using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Enums;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;
using ProductionPlanner.ViewModels;

namespace ProductionPlanner.ViewComponents
{
    public class ProjectColumnViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ProjectColumnViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(List<Week>? weeks)
        {
            PlannerViewModel plannerViewModel = new()
            {
                Weeks = weeks ?? await GetWeeks()
            };

            return View(plannerViewModel);
        }

        private Task<List<Week>> GetWeeks()
        {
            return _db.Weeks.Include(w => w.Days).ThenInclude(d => d.Tasks).ToListAsync();
        }
    }
}