using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionPlanner.Models
{
    public class Day
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid Guid { get; set; } = Guid.NewGuid();
        public List<ProjectTask> Tasks { get; set; }
        public DateTime Date { get; set; }
        public int AvailableHours { get; set; }
        public int HoursLeftToBook { get; set; }
        public string? DayName { get; set; }
        public int Priority { get; set; }
    }
}