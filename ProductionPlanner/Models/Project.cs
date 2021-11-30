using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductionPlanner.Models
{
    public class Project
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
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
        public List<ProjectTask> GetSortedProjectTasks()
        {
            return Tasks.OrderBy(t => t.Priority).ToList();
        }
    }
}