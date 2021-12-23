using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductionPlanner.Models
{
    public class ProjectTemplate
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; }
    }
}