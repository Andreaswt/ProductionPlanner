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
                new Day { DayName = "Monday", AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 1) },
                new Day { DayName = "Tuesday", AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 2) },
                new Day { DayName = "Wednesday", AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 3) },
                new Day { DayName = "Thursday", AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 4) },
                new Day { DayName = "Friday", AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 5) },
                /*new Day { DayName = "Saturday", AvailableHours = 8 },
                new Day { DayName = "Sunday", AvailableHours = 8 },*/
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