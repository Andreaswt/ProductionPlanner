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
            var existing = _db.ProjectTemplates.Include(p => p.ProjectTasks).First(x => x.Name == projectTemplate.Name);
            
            // Remove existing project tasks, so they don't clutter DB
            _db.ProjectTasks.RemoveRange(existing.ProjectTasks);
            
            // Update project task name for each project task
            foreach (var projectTask in projectTemplate.ProjectTasks)
            {
                projectTask.ProjectName = projectTemplate.Name;
            }

            projectTemplate.Owner = "Admin"; // TODO: get logged in user name

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
            Project project = templateToProjectMapper(projectTemplate);
            project.Owner = "Admin";
            
            _db.Projects.Add(project);
            _db.SaveChanges();
            
            return true;
        }
        
        public bool CreateProjectFromScratch(ProjectTemplate projectTemplate)
        {
            // If project with name already exists, return false
            if (_db.Projects.Any(x => x.Name == projectTemplate.Name))
                return false;
            
            // Map relevant properties
            Project project = templateToProjectMapper(projectTemplate);

            project.Owner = "Admin";
            
            // TODO: Get the highest priority project, and make the new project +1 priority.
            //.Select(s => s.OrderByDescending(a => a.Version).First());
            var record = _db.Projects.Select(p => p.Priority);

            _db.Projects.Add(project);
            _db.SaveChanges();
            
            return true;
        }

        private Project templateToProjectMapper(ProjectTemplate projectTemplate)
        {
            Project project = new Project
            {
                Name = projectTemplate.Name,
                Description = projectTemplate.Description,
                Owner = projectTemplate.Owner,
                ProjectTasks = projectTemplate.ProjectTasks,
            };
            
            // Set project name in project task
            foreach (var projectTask in project.ProjectTasks)
            {
                projectTask.ProjectName = project.Name;
            }

            return project;
        }
    }
}