using System.Collections.Generic;
using ProductionPlanner.Models;

namespace ProductionPlanner.ViewModels
{
    public class ProjectTemplatesViewModel
    {
        public List<ProjectTemplate> ProjectTemplates { get; set; }
        
        // New project template, being posted back to controller
        public ProjectTemplate ProjectTemplate { get; set; }
    }
}