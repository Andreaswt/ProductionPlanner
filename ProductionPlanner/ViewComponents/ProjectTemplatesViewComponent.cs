using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Models;
using ProductionPlanner.ViewModels;

namespace ProductionPlanner.ViewComponents
{
    public class ProjectTemplatesViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ProjectTemplatesViewComponent(ApplicationDbContext applicationDbContext)
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
            // Add mock templates to DB
            // SaveMockProjectTemplates();
            
            return _db.ProjectTemplates.Include(p => p.ProjectTasks).ToListAsync();
        }

        private void SaveMockProjectTemplates()
        {
            var projectTemplates = new List<ProjectTemplate>();

            ProjectTemplate p1 = new ProjectTemplate
            {
                Name = "ProjectTemplate1",
                Description = "This is a tempalte for model xyz",
                Owner = "Admin",
                ProjectTasks = new List<ProjectTask>
                {
                    new ProjectTask
                    {
                        Priority = 1,
                        Duration = 10,
                        Name = "Opg 1",
                        Description = "Opgave 1",
                        ProjectName = "Projekt 1",
                        Progress = "Todo",
                        PersonAssigned = "Donald",
                    },
                    new ProjectTask
                    {
                        Priority = 2,
                        Duration = 4,
                        Name = "Opg 2",
                        Description = "Opgave 2",
                        ProjectName = "Projekt 1",
                        Progress = "Todo",
                        PersonAssigned = "Donald"
                    },
                    new ProjectTask
                    {
                        Priority = 3,
                        Duration = 1,
                        Name = "Opg 3",
                        Description = "Opgave 3",
                        ProjectName = "Projekt 1",
                        Progress = "Todo",
                        PersonAssigned = "Donald"
                    }
                }
            };
            
            ProjectTemplate p2 = new ProjectTemplate
            {
                Name = "ProjectTemplate1",
                Description = "This is a tempalte for model xyz",
                Owner = "Admin",
                ProjectTasks = new List<ProjectTask>
                {
                    new ProjectTask
                    {
                        Priority = 4,
                        Duration = 1,
                        Name = "Opg 4",
                        Description = "Opgave 4",
                        ProjectName = "Projekt 1",
                        Progress = "Todo",
                        PersonAssigned = "Donald"
                    },

                    new ProjectTask
                    {
                        Priority = 1,
                        Duration = 2,
                        Name = "Opg 5",
                        Description = "Opgave 5",
                        ProjectName = "Projekt 2",
                        Progress = "Todo",
                        PersonAssigned = "Donald"
                    },
                    new ProjectTask
                    {
                        Priority = 2,
                        Duration = 3,
                        Name = "Opg 6",
                        Description = "Opgave 6",
                        ProjectName = "Projekt 2",
                        Progress = "Todo",
                        PersonAssigned = "Donald"
                    }
                }
            };
            
            projectTemplates.Add(p1);
            projectTemplates.Add(p2);
            
            _db.ProjectTemplates.UpdateRange(projectTemplates);
            _db.SaveChanges();
        }
    }
}