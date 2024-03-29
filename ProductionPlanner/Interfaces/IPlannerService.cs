using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IPlannerService
    {
        public List<Week> AssignProjects(List<Project> projects);
        public List<Project> MockData();
    }
}