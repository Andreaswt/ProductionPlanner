using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;

namespace ProductionPlanner.Services
{
    public class PlannerService : IPlannerService
    {
        Week week = new();

        public List<Week> AssignProjects(List<Project> projects)
        {
            var sortedProjects = projects.OrderBy(t => t.Priority).ToList();
            List<ProjectTask> sortedTasks = new();

            // Long list of tasks sorted by priority
            foreach (Project project in sortedProjects)
            {
                foreach (ProjectTask projectTask in project.Tasks)
                {
                    sortedTasks.Add(projectTask);
                }
            }

            List<Week> weeks = new();

            Week week = new();
            foreach (ProjectTask task in sortedTasks)
            {
                if (!task.Assigned)
                {
                    // Check if all days are assigned, otherwise add new week
                    if (week.HoursAvailable == 0)
                    {
                        weeks.Add(week);
                        week = new();
                    }
                    
                    foreach (Day day in week.Days.OrderBy(d => d.Priority).ToList())
                    {
                        // Assign task to day
                        if (day.HoursLeftToBook > task.Duration)
                        {
                            day.Tasks.Add(task);
                            day.HoursLeftToBook -= task.Duration;
                            task.Assigned = true;
                            break;
                        }
                    }
                }
            }
            if (weeks.Count == 0)
                weeks.Add(week);

            return weeks;
        }

        public List<Project> MockData() 
        {
            ProjectTask t = new ProjectTask
            {
                Priority = 1,
                Duration = 2,
                Name = "Opg 1",
                Description = "Opgave 1",
                ProjectName = "Projekt 1",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t1 = new ProjectTask
            {
                Priority = 2,
                Duration = 4,
                Name = "Opg 2",
                Description = "Opgave 2",
                ProjectName = "Projekt 1",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t2 = new ProjectTask
            {
                Priority = 3,
                Duration = 1,
                Name = "Opg 3",
                Description = "Opgave 3",
                ProjectName = "Projekt 1",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t3 = new ProjectTask
            {
                Priority = 4,
                Duration = 1,
                Name = "Opg 4",
                Description = "Opgave 4",
                ProjectName = "Projekt 1",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };

            ProjectTask t4 = new ProjectTask
            {
                Priority = 1,
                Duration = 2,
                Name = "Opg 5",
                Description = "Opgave 5",
                ProjectName = "Projekt 2",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t5 = new ProjectTask
            {
                Priority = 2,
                Duration = 3,
                Name = "Opg 6",
                Description = "Opgave 6",
                ProjectName = "Projekt 2",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t6 = new ProjectTask
            {
                Priority = 3,
                Duration = 3,
                Name = "Opg 7",
                Description = "Opgave 7",
                ProjectName = "Projekt 2",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t7 = new ProjectTask
            {
                Priority = 4,
                Duration = 10,
                Name = "Opg 8",
                Description = "Opgave 8",
                ProjectName = "Projekt 2",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };

            ProjectTask t8 = new ProjectTask
            {
                Priority = 1,
                Duration = 6,
                Name = "Opg 9",
                Description = "Opgave 9",
                ProjectName = "Projekt 3",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t9 = new ProjectTask
            {
                Priority = 2,
                Duration = 2,
                Name = "Opg 10",
                Description = "Opgave 10",
                ProjectName = "Projekt 3",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t10 = new ProjectTask
            {
                Priority = 3,
                Duration = 3,
                Name = "Opg 11",
                Description = "Opgave 11",
                ProjectName = "Projekt 3",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };
            ProjectTask t11 = new ProjectTask
            {
                Priority = 4,
                Duration = 5,
                Name = "Opg 12",
                Description = "Opgave 12",
                ProjectName = "Projekt 3",
                Progress = "Todo",
                PersonAssigned = "Donald"
            };

            List<ProjectTask> pt1 = new List<ProjectTask>();
            pt1.Add(t);
            pt1.Add(t1);
            pt1.Add(t2);
            pt1.Add(t3);

            List<ProjectTask> pt2 = new List<ProjectTask>();
            pt2.Add(t4);
            pt2.Add(t5);
            pt2.Add(t6);
            pt2.Add(t7);

            List<ProjectTask> pt3 = new List<ProjectTask>();
            pt3.Add(t8);
            pt3.Add(t9);
            pt3.Add(t10);
            pt3.Add(t11);


            Project project1 = new Project
            {
                Name = "Mockproject 1",
                Priority = 1,
                Tasks = pt1
            };

            Project project2 = new Project
            {
                Name = "Mockproject 2",
                Priority = 2,
                Tasks = pt2
            };

            Project project3 = new Project
            {
                Name = "Mockproject 3",
                Priority = 3,
                Tasks = pt3
            };

            List<Project> projectList = new List<Project>();
            projectList.Add(project1);
            projectList.Add(project2);
            projectList.Add(project3);

            return projectList;
        }
    }
}