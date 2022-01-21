using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IPlannerService
    {
        public List<Week> AssignProjects(bool saveToDB);
        public List<Project> MockData();
    }
}