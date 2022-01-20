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
    public class ProjectDetailsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ProjectDetailsViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            ProjectDetailsViewModel projectDetailsViewModel = new ProjectDetailsViewModel
            {
                Project = await GetProject(id),
                TotalProjectCount = await GetTotalProjectCount()
            };
            
            projectDetailsViewModel.Project.ProjectTasks.Sort();

            return View(projectDetailsViewModel);
        }

        private Task<Project?> GetProject(int id)
        {
            return _db.Projects.Include(p => p.ProjectTasks).FirstOrDefaultAsync(p => p.Id == id);
        }

        private Task<int> GetTotalProjectCount()
        {
            return _db.Projects.CountAsync();
        }
    }
}