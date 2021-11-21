using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Models
{
    public class Project
    {
        public string Name { get; set; }
        
        public int Priority { get; set; }
        
        public List<ProjectTask> Tasks { get; set; }

        public int TotalHours
        {
            get
            {
                int totalHours = 0;
                foreach (ProjectTask task in Tasks)
                {
                    totalHours += task.Duration;
                }
                return totalHours;
            }
        }

        public int TotalHoursWithoutBufferTimer { get; set; }

        public DateTime StartDate
        {
            get
            {
                List<DateTime> allStartDates = new();
                foreach (ProjectTask task in Tasks)
                {
                    allStartDates.Add(task.StartDate);
                }
                return allStartDates.Min();
            }
        }

        public DateTime EndDate
        {
            get
            {
                List<DateTime> allEndDates = new();
                foreach (ProjectTask task in Tasks)
                {
                    allEndDates.Add(task.StartDate);
                }
                return allEndDates.Max();
            }
        }

        public bool Started { get; set; }

        public bool Finished { get; set; }

        public bool AllTasksAssigned { get; set; }
        
        public List<ProjectTask> GetSortedProjectTasks()
        {
            return Tasks.OrderBy(t => t.Priority).ToList();
        }
    }
}