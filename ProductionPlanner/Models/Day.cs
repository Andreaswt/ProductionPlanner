using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductionPlanner.Models
{
    public class Day
    {
        [Key]
        public int Id { get; set; }
        public List<ProjectTask>? Tasks { get; set; }
        public DateTime Date { get; set; }
        public int AvailableHours { get; set; }
        public int HoursLeftToBook { get; set; }
        public DayOfWeek? DayName { get; set; }
    }
}