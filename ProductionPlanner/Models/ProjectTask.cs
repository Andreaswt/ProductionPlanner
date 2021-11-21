using System;
using System.Collections.Generic;

namespace ProductionPlanner.Models
{
    public class ProjectTask
    {
        public int Priority { get; set; }

        public int Duration { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool BufferTimer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Assigned { get; set; }
    }
}