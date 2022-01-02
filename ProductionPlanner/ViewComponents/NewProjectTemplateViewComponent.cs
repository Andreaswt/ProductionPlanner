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
            ProjectTemplate p = new();
            return Task.FromResult<IViewComponentResult>(View(p));
        }
    }
}