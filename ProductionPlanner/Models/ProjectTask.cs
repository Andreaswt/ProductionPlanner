using System;
using System.Collections.Generic;

namespace ProductionPlanner.Models
{
    public class ProjectTask
    {
        public Guid Guid { get; set; } = Guid.NewGuid();
        public int Priority { get; set; }
        public int Duration { get; set; }
        public DateTime Date { get; set; }
        public int StartHour { get; set; }
        public int EndHour { get; set; }
        public bool BufferTimer { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Assigned { get; set; }
        public Guid ParentGuid { get; set; }
    }
}