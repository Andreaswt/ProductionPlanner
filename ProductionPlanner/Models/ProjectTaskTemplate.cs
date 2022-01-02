using System;
using System.ComponentModel.DataAnnotations;

namespace ProductionPlanner.Models
{
    public class ProjectTaskTemplate
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid task name")]
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid priority")]
        public int Priority { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid duration")]
        public int Duration { get; set; }
    }
}