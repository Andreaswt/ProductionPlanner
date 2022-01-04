using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ProductionPlanner.Models
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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