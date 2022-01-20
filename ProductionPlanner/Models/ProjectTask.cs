using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProductionPlanner.Enums;

namespace ProductionPlanner.Models
{
    public class ProjectTask : IComparable<ProjectTask>
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid name.")]
        public string? Name { get; set; }
        public string? ProjectName { get; set; }
        public ProjectTaskProgress Progress { get; set; } = ProjectTaskProgress.Todo;
        
        [Required(ErrorMessage = "Please enter a valid description.")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int Priority { get; set; }
        
        [Range(0, int.MaxValue)]
        [Required(ErrorMessage = "Please enter a valid duration.")]
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public bool Assigned { get; set; }
        public string? PersonAssigned { get; set; }
        public bool Subtask { get; set; }
        public int CompareTo(ProjectTask? other)
        {
            if (other.Priority > this.Priority)
                return -1;
            
            if (other.Priority < this.Priority)
                return 1;
            
            return 0;
        }
    }
}