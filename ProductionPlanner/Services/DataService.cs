using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Enums;
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

        public List<Project> GetProjects()
        {
            return _db.Projects.Include(p => p.ProjectTasks).OrderBy(t => t.Priority).ToList();
        }
        
        public void SaveWeeks(List<Week> weeks)
        {
            // Check if weeks updated in algorithm contains same week numbers
            var dbWeeks = _db.Weeks.Include(w => w.Days);
            foreach (var week in weeks)
            {
                // Loop through DB weeks
                foreach (var weekDb in dbWeeks)
                {
                    if (weekDb.WeekNo == week.WeekNo)
                    {
                        
                        // Update existing dates instead of creating new ones
                        foreach (var dayDb in weekDb.Days)
                        {
                            if (weekDb.Days.Any(d => d.Date == dayDb.Date))
                            {
                                var day = weekDb.Days.FirstOrDefault(d => d.Date == dayDb.Date);
                                dayDb.Priority = day.Priority;
                                dayDb.Tasks = day.Tasks;
                                dayDb.AvailableHours = day.AvailableHours;
                                dayDb.HoursLeftToBook = day.HoursLeftToBook;
                            }
                        }
                        
                        weekDb.Ferie = week.Ferie;
                        weekDb.WeekNo = week.WeekNo;
                        weekDb.Year = week.Year;
                        _db.Weeks.Update(weekDb);
                    }
                    else
                        _db.Weeks.Add(week); // TODO: sørg for at uger ikke bliver tilføjet hver gang der trykkes update
                }
            }

            if (!dbWeeks.Any())
                _db.Weeks.UpdateRange(weeks);

            _db.SaveChanges();
        }

        public List<Week> GetWeeks()
        {
            // Eager load related data
            return _db.Weeks.Include(w => w.Days).ThenInclude(d => d.Tasks).ToList();
        }

        public Day? GetDay(DateTime date)
        {
            return _db.Days.FirstOrDefault(d => d.Date == date);
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

        public bool CreateProject(ProjectTemplate projectTemplate)
        {
            // If project with name already exists, return false
            if (_db.Projects.Any(x => x.Name == projectTemplate.Name))
                return false;
            
            // Map relevant properties
            Project project = TemplateToProjectMapper(projectTemplate);
            project.Owner = "Admin";
            project.Priority = GetNextProjectPriority();
            project.Created = DateTime.Now;
            
            _db.Projects.Add(project);
            _db.SaveChanges();
            
            return true;
        }
        
        public void UpdateProjectProgress(int id, ProjectProgress projectProgress)
        {
            // If project with name already exists, return false
            var project = _db.Projects.FirstOrDefault(p => p.Id == id);
            project.Progress = projectProgress;
                
            _db.Projects.Update(project);
            _db.SaveChanges();
        }
        
        public void UpdateProjectPriority(int id, int projectPriority)
        {
            // Don't allow inputted priority to be more than 1 larger than the largest priority project
            var highestPriority = _db.Projects.OrderByDescending(x => x.Priority).FirstOrDefault().Priority;
            
            if ((projectPriority - highestPriority) > 1)
                projectPriority = (int) highestPriority;

            // Change priority of other rows affected
            var project = _db.Projects.FirstOrDefault(p => p.Id == id);
            var projects = projectPriority < project.Priority ? _db.Projects.Where(p => p.Priority >= projectPriority && p.Priority < project.Priority) : _db.Projects.Where(p => p.Priority <= projectPriority && p.Priority > project.Priority);
            
            foreach (var project1 in projects)
            {
                if (projectPriority < project.Priority)
                    project1.Priority += 1;
                else
                    project1.Priority -= 1;
            }

            project.Priority = projectPriority;
            
            _db.Projects.Update(project);
            _db.Projects.UpdateRange(projects);
            _db.SaveChanges();
        }
        
        public void UpdateProjectTaskProgress(int id, ProjectTaskProgress projectProgress)
        {
            // If project with name already exists, return false
            var projectTask = _db.ProjectTasks.FirstOrDefault(p => p.Id == id);
            projectTask.Progress = projectProgress;
                
            _db.ProjectTasks.Update(projectTask);
            _db.SaveChanges();
        }

        private Project TemplateToProjectMapper(ProjectTemplate projectTemplate)
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

        private int? GetNextProjectPriority()
        {
            // Get the highest priority project, and make the new project +1 priority.
            var lowestPriorityItem = _db.Projects.OrderByDescending(x => x.Priority).FirstOrDefault();
            var topPriority  = lowestPriorityItem == null ? 0 : lowestPriorityItem.Priority;

            return topPriority + 1;
        }
    }
}