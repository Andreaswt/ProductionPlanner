using System.Collections.Generic;
using ProductionPlanner.Models;

namespace ProductionPlanner.ViewModels
{
    public class ProjectListViewModel
    {
        public List<Project> Projects { get; set; }
        public int TotalProjects { get; set; }
        public int TodoProjects { get; set; }
        public int InProgressProjects { get; set; }
        public int FinishedProjects { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public bool ShowTodo { get; set; } = true;
        public bool ShowInProgress { get; set; } = true;
        public bool ShowFinished { get; set; } = true;
    }
}