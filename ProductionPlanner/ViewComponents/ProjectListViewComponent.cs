using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionPlanner.Data;
using ProductionPlanner.Enums;
using ProductionPlanner.Models;
using ProductionPlanner.ViewModels;

namespace ProductionPlanner.ViewComponents
{
    public class ProjectListViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public ProjectListViewComponent(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public async Task<IViewComponentResult> InvokeAsync(bool showTodo = true, bool showInProgress = true, bool showFinished = true, int page = 0, int pagesize = 15)
        {
            ProjectListViewModel projectListViewModel = new ProjectListViewModel
            {
                Projects = await GetProjects(page, pagesize, showTodo, showInProgress, showFinished),
                TotalProjects = await GetProjectCount(),
                TodoProjects = await GetTodoProjectCount(),
                InProgressProjects = await GetInProgressProjectCount(),
                FinishedProjects = await GetFinishedProjectCount(),
                From = page * pagesize,
                To = page * pagesize + pagesize,
                Page = page,
                ShowTodo = showTodo,
                ShowInProgress = showInProgress,
                ShowFinished = showFinished
            };
            
            if (showTodo && showInProgress && showFinished)
                projectListViewModel.TotalPages = (int) Math.Ceiling((double) projectListViewModel.TotalProjects / (double) pagesize);
            else if (showTodo)
                projectListViewModel.TotalPages = (int) Math.Ceiling((double) projectListViewModel.TodoProjects / (double) pagesize);
            else if (showInProgress)
                projectListViewModel.TotalPages = (int) Math.Ceiling((double) projectListViewModel.InProgressProjects / (double) pagesize);
            else if (showFinished)
                projectListViewModel.TotalPages = (int) Math.Ceiling((double) projectListViewModel.FinishedProjects / (double) pagesize);

            return View(projectListViewModel);
        }

        private Task<List<Project>> GetProjects(int page, int pagesize, bool todo, bool inprogress, bool finished)
        {
            if (todo && inprogress && finished)
                return _db.Projects.Include(p => p.ProjectTasks).OrderBy(p => p.Priority).Skip(page*pagesize).Take(pagesize).ToListAsync();
            else if (todo)
                return _db.Projects.Include(p => p.ProjectTasks).OrderBy(p => p.Priority).Where(p => p.Progress == ProjectProgress.Todo).Skip(page*pagesize).Take(pagesize).ToListAsync();
            else if (inprogress)
                return _db.Projects.Include(p => p.ProjectTasks).OrderBy(p => p.Priority).Where(p => p.Progress == ProjectProgress.InProgress).Skip(page*pagesize).Take(pagesize).ToListAsync();
            else if (finished)
                return _db.Projects.Include(p => p.ProjectTasks).OrderBy(p => p.Priority).Where(p => p.Progress == ProjectProgress.Finished).Skip(page*pagesize).Take(pagesize).ToListAsync();
            
            // Default: return all projects
            else
                return _db.Projects.Include(p => p.ProjectTasks).OrderBy(p => p.Priority).Skip(page*pagesize).Take(pagesize).ToListAsync(); 
        }
        
        private Task<int> GetProjectCount()
        {
            return _db.Projects.CountAsync();
        }

        private Task<int> GetTodoProjectCount()
        {
            return _db.Projects.CountAsync(p => p.Progress == ProjectProgress.Todo);
        }
        
        private Task<int> GetInProgressProjectCount()
        {
            return _db.Projects.CountAsync(p => p.Progress == ProjectProgress.InProgress);
        }
        
        private Task<int> GetFinishedProjectCount()
        {
            return _db.Projects.CountAsync(p => p.Progress == ProjectProgress.Finished);
        }
    }
}