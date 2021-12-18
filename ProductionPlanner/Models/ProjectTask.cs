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
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public string? ProjectName { get; set; }
        public string? Progress { get; set; }
        public string? Description { get; set; }
        public int Priority { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public bool Assigned { get; set; }
        public string? PersonAssigned { get; set; }
        public bool Subtask { get; set; }
    }
}