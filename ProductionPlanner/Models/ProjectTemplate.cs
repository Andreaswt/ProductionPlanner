using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductionPlanner.Models
{
    public class ProjectTemplate
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid project name")]
        public string? Name { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid project description")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        public string? Owner { get; set; }
        public List<ProjectTask>? ProjectTasks { get; set; }
    }
}