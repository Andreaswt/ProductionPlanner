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
    public class EditProjectTemplateViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public EditProjectTemplateViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            ProjectTemplatesViewModel projectTemplatesViewModel = new ProjectTemplatesViewModel
            {
                ProjectTemplate = await GetProjectTemplates(id)
            };

            return View(projectTemplatesViewModel);
        }

        private Task<ProjectTemplate?> GetProjectTemplates(int id)
        {
            return _db.ProjectTemplates.Include(p => p.ProjectTasks).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}