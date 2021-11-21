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
        public List<Project> AssignProjects(List<Project> projects)
        {
            var sortedProjects = projects.OrderBy(t => t.Priority).ToList();

            Week week = new Week();
            
            foreach (Day day in week.Days)
            {
                var nextAvailableTask = GetNextAvailableTask(sortedProjects);
                if (day.HoursLeftToBook > nextAvailableTask.Duration)
                {
                    day.HoursLeftToBook -= nextAvailableTask.Duration;

                    nextAvailableTask.StartDate = day.Date;
                    nextAvailableTask.EndDate = day.Date;
                    nextAvailableTask.Assigned = true;
                }
            }
            
            List<Project> remainingProjects = new List<Project>();
            return remainingProjects;
        }

        private ProjectTask GetNextAvailableTask(List<Project> projects)
        {
            // Get highest priority project in list of projects
            Project highestPriorityProject = new();
            ProjectTask highestPriorityProjectTask = new();
            foreach (Project project in projects)
            {
                if (!project.AllTasksAssigned)
                {
                    highestPriorityProject = project;
                }
                    
                // Get highest priority task in project
                foreach (ProjectTask projectTask in highestPriorityProject.Tasks)
                {
                    if (!projectTask.Assigned)
                    {
                        highestPriorityProjectTask = projectTask;
                        break;
                    }
                }
            }
            return highestPriorityProjectTask;
        }
        
        public string ChartJsonGenerator(List<Project> projects)
        {
            var tasks = new[]
            {
                new {RowLabel = "Projekt 1", BarLabel = "Mal", Start = new DateTime(2021, 12, 1), End = new DateTime(2021, 12, 3)},
                new {RowLabel = "Projekt 1", BarLabel = "Mal", Start = new DateTime(2021, 12, 4), End = new DateTime(2021, 12, 5)},
                new {RowLabel = "Projekt 2", BarLabel = "Støb", Start = new DateTime(2021, 12, 1), End = new DateTime(2021, 12, 2)},
                new {RowLabel = "Projekt 2", BarLabel = "Støb", Start = new DateTime(2021, 12, 2), End = new DateTime(2021, 12, 3)},
                new {RowLabel = "Projekt 3", BarLabel = "Støb", Start = new DateTime(2021, 12, 1), End = new DateTime(2021, 12, 14)},
                new {RowLabel = "Projekt 4", BarLabel = "Støb", Start = new DateTime(2021, 12, 1, 0, 0, 0), End = new DateTime(2021, 12, 1, 23, 0, 0)},
            };

            List<TimelineRow> timelineFormat = new List<TimelineRow>();

            foreach (Project project in projects)
            {
                foreach (ProjectTask projectTask in project.Tasks)
                {
                    TimelineRow t = new()
                    {
                        BarLabel = project.Name,
                        RowLabel = projectTask.Name,
                        Start = projectTask.StartDate,
                        End = projectTask.EndDate,
                    };
                    
                    timelineFormat.Add(t);
                }
            }
            
            var json = timelineFormat.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Row label"), x => x.RowLabel)
                .NewColumn(new Column(ColumnType.String, "Bar label"), x => x.BarLabel)
                .NewColumn(new Column(ColumnType.Datetime, "Start"), x => x.Start)
                .NewColumn(new Column(ColumnType.Datetime, "End"), x => x.End)
                .Build()
                .GetJson();

            return json;
        }

        public List<Project> MockData()
        {
            ProjectTask t = new ProjectTask
            {
                Priority = 1,
                Duration = 5,
                Name = "Opg 1",
                Description = "Opgave 1",
            };
            ProjectTask t1 = new ProjectTask
            {
                Priority = 2,
                Duration = 2,
                Name = "Opg 2",
                Description = "Opgave 2",
            };
            ProjectTask t2 = new ProjectTask
            {
                Priority = 3,
                Duration = 3,
                Name = "Opg 3",
                Description = "Opgave 3",
            };
            ProjectTask t3 = new ProjectTask
            {
                Priority = 4,
                Duration = 10,
                Name = "Opg 3",
                Description = "Opgave 3",
            };
            ProjectTask t4 = new ProjectTask
            {
                Priority = 5,
                Duration = 8,
                Name = "Opg 4",
                Description = "Opgave 4",
            };
            ProjectTask t5 = new ProjectTask
            {
                Priority = 6,
                Duration = 3,
                Name = "Opg 5",
                Description = "Opgave 5",
            };
            ProjectTask t6 = new ProjectTask
            {
                Priority = 7,
                Duration = 15,
                Name = "Opg 6",
                Description = "Opgave 6",
            };
            ProjectTask t7 = new ProjectTask
            {
                Priority = 8,
                Duration = 6,
                Name = "Opg 7",
                Description = "Opgave 7",
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

            Project project1 = new Project
            {
                Priority = 1,
                Tasks = pt1
            };
            
            Project project2 = new Project
            {
                Priority = 2,
                Tasks = pt2
            };

            List<Project> projectList = new List<Project>();
            projectList.Add(project1);
            projectList.Add(project2);

            return projectList;
        }
    }
}