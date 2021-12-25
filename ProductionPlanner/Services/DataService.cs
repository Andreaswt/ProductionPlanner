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

        public void SaveProjectTemplate(ProjectTemplate projectTemplate)
        {
            foreach (var projectTask in projectTemplate.ProjectTasks)
            {
                projectTask.ProjectName = projectTemplate.Name;
            }

            projectTemplate.Owner = "Admin"; // TODO: get logged in user name

            _db.ProjectTemplates.Add(projectTemplate);
            _db.SaveChanges();
        }
        
        public void DeleteProjectTemplate(int id)
        {
            ProjectTemplate p = new ProjectTemplate
            {
                Id = id
            };
            _db.ProjectTemplates.Remove(p);
            _db.SaveChanges();
        }
    }
}