using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionPlanner.Models
{
    public class ProjectTask
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Please enter a valid task name")]
        public string? Name { get; set; }
        public string? ProjectName { get; set; }
        public string? Progress { get; set; }
        
        public string? Description { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid priority")]
        public int Priority { get; set; }
        
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid duration")]
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public bool Assigned { get; set; }
        public string? PersonAssigned { get; set; }
        public bool Subtask { get; set; }
    }
}