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
    public class NewProjectTemplateViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public NewProjectTemplateViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            ProjectTemplate projectTemplate = new();
            
            // First projecttask must have the priority 0.
            // Bigger priorities will be added by JS in form
            projectTemplate.ProjectTasks = new List<ProjectTask>();
            projectTemplate.ProjectTasks.Add(new ProjectTask());

            return Task.FromResult<IViewComponentResult>(View(projectTemplate));
        }
    }
}