using System.Collections.Generic;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IDataService
    {
        public void SaveWeeks(List<Week> weeks);
        public List<Week> GetWeeks();
        public bool SaveProjectTemplate(ProjectTemplate projectTemplate);
        public void EditProjectTemplate(ProjectTemplate projectTemplate);
        public void DeleteProjectTemplate(int id);
        public bool CreateProject(ProjectTemplate projectTemplate);
    }
}