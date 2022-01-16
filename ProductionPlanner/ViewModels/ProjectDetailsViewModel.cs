using ProductionPlanner.Models;

namespace ProductionPlanner.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public Project? Project { get; set; }
        
        public int TotalProjectCount { get; set; }
    }
}