using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;

namespace ProductionPlanner.Services
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _db;

        public DataService(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        
        public void SaveWeeks(List<Week> weeks)
        {
            _db.Weeks.AddRange(weeks);
            _db.SaveChanges();
        }

        public List<Week> GetWeeks()
        {
            // Eager load related data
            return _db.Weeks.Include(w => w.Days).ThenInclude(d => d.Tasks).ToList();
        }

        public bool SaveProjectTemplate(ProjectTemplate projectTemplate)
        {
            // If project with name already exists, return false
            if (_db.ProjectTemplates.Any(x => x.Name == projectTemplate.Name))
                return false;
            
            // Otherwise create the projectTemplate
            foreach (var projectTask in projectTemplate.ProjectTasks)
            {
                projectTask.ProjectName = projectTemplate.Name;
            }

            projectTemplate.Owner = "Admin"; // TODO: get logged in user name

            _db.ProjectTemplates.Add(projectTemplate);
            _db.SaveChanges();

            return true;
        }
        
        public void EditProjectTemplate(ProjectTemplate projectTemplate)
        {
            // Get existing
            var existing = _db.ProjectTemplates.First(x => x.Name == projectTemplate.Name);

            // Update inputted changes from form
            existing.Description = projectTemplate.Description;
            existing.ProjectTasks = projectTemplate.ProjectTasks;
            
            // Store the updated object
            _db.ProjectTemplates.Update(existing);
            _db.SaveChanges();
        }
        
        public void DeleteProjectTemplate(int id)
        {
            ProjectTemplate p = _db.ProjectTemplates.Include(p => p.ProjectTasks).First(x => x.Id == id);
            _db.ProjectTasks.RemoveRange(p.ProjectTasks);
            _db.ProjectTemplates.Remove(p);
            _db.SaveChanges();
        }
        
        public bool CreateProjectFromTemplate(ProjectTemplate projectTemplate)
        {
            // If project with name already exists, return false
            if (_db.Projects.Any(x => x.Name == projectTemplate.Name))
                return false;
            
            // Map relevant properties
            Project project = new Project
            {
                Name = projectTemplate.Name,
                Description = projectTemplate.Description,
                Owner = projectTemplate.Description,
                ProjectTasks = projectTemplate.ProjectTasks
            };
            
            _db.Projects.Add(project);
            _db.SaveChanges();
            
            return true;
        }
    }
}