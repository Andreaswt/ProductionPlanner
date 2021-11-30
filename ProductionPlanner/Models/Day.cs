using System;
using System.Collections.Generic;

namespace ProductionPlanner.Models
{
    public class Day
    {
        public List<ProjectTask> Tasks { get; set; } = new();
        public DateTime Date { get; set; }
        public int AvailableHours { get; set; }
        public int HoursLeftToBook { get; set; }
        public string DayName { get; set; }
        public int Priority { get; set; }
        public void AddTasks(List<ProjectTask> tasks)
        {
            foreach (ProjectTask task in tasks)
            {
                Tasks.Add(task);
            }
        }
    }
}