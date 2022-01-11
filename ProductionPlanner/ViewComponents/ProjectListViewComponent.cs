using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Models;
using ProductionPlanner.ViewModels;

namespace ProductionPlanner.ViewComponents
{
    public class ProjectListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ProjectListViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Project> projects = await GetProjects();

            return View(projects);
        }

        private Task<List<Project>> GetProjects()
        {
            return _db.Projects.Include(p => p.ProjectTasks).OrderBy(p => p.Priority).ToListAsync();
        }
    }
}