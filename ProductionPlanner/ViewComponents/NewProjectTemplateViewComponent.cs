using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Models;
using ProductionPlanner.ViewModels;

namespace ProductionPlanner.ViewComponents
{
    public class NewProjectTemplateViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public NewProjectTemplateViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ProjectTemplatesViewModel projectTemplatesViewModel = new ProjectTemplatesViewModel
            {
                ProjectTemplates = await GetProjectTemplates()
            };

            return View(projectTemplatesViewModel);
        }

        private Task<List<ProjectTemplate>> GetProjectTemplates()
        {
            return _db.ProjectTemplates.Include(p => p.ProjectTasks).ToListAsync();
        }
    }
}