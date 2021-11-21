using System;
using System.Collections.Generic;

namespace ProductionPlanner.Models
{
    public class Day
    {
        public Day()
        {
            HoursLeftToBook = AvailableHours;
        }
        
        public List<ProjectTask> Tasks { get; set; }

        public DateTime Date { get; set; }

        public int AvailableHours { get; set; }
        
        public int HoursLeftToBook { get; set; }

        public string DayName { get; set; }

        public void AddTasks(List<ProjectTask> tasks)
        {
            foreach (ProjectTask task in tasks)
            {
                Tasks.Add(task);
            }
        }
    }
}