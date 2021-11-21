using System;

namespace ProductionPlanner.Models
{
    public class TimelineRow
    {
        public string RowLabel { get; set; }
        public string BarLabel { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}