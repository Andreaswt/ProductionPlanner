using System.Collections.Generic;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IDataService
    {
        public void SaveWeeks(List<Week> weeks);
        public List<Week> GetWeeks();
        public void SaveProjectTemplate(ProjectTemplate projectTemplate);
        public void DeleteProjectTemplate(int id);
    }
}