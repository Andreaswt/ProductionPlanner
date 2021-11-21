using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IPlannerService
    {
        public List<Project> AssignProjects(List<Project> projects);

        public string ChartJsonGenerator(List<Project> projects);
        
        public List<Project> MockData();
    }
}