using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ProductionPlanner.Models
{
    public class Week
    {
        public Week()
        {
            Days = new List<Day>
            {
                new Day { DayName = "Monday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 1) },
                new Day { DayName = "Tuesday", Priority = 2, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 2) },
                new Day { DayName = "Wednesday", Priority = 3, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 3) },
                new Day { DayName = "Thursday", Priority = 4, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 4) },
                new Day { DayName = "Friday", Priority = 5, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 5) },
                new Day { DayName = "Saturday", Priority = 6, AvailableHours = 0, HoursLeftToBook = 0, Date = new DateTime(2021, 12, 6) },
                new Day { DayName = "Sunday", Priority = 7, AvailableHours = 0, HoursLeftToBook = 0, Date = new DateTime(2021, 12, 7) },
            };
        }
        public List<Project> Projects { get; set; }
        public List<Day> Days { get; set; }
        public int HoursAvailable
        {
            get
            {
                var totalHours = 0;
                foreach (Day day in Days)
                {
                    totalHours += day.AvailableHours;
                }

                return totalHours;
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Ferie { get; set; }
        public int WeekNo { get; set; }
        public void AddProjects(List<Project> projects)
        {
            foreach (Project p in projects)
            {
                Projects.Add(p);
            }
        }
        public List<ProjectTask> GetAllTasksThisWeek()
        {
            List<ProjectTask> projectTasks = new List<ProjectTask>();

            foreach (Project project in Projects)
            {
                foreach (ProjectTask projectTask in project.GetSortedProjectTasks())
                {
                    projectTasks.Add(projectTask);
                }
            }
            
            return projectTasks;
        }
        public List<ProjectTask> AssignProjects()
        {
            List<ProjectTask> remainingProjects = new List<ProjectTask>();
            return remainingProjects;
        }
    }
}